using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SnakeBattle.AI
{
    public class AIController
    {
        public static int maxDeep = 10;
        public static async Task<SnakeAction?> GetOptimalActionAsync(GameBoard gameBoard, SnakeAction lastMove)
        {
            var tasks = new Dictionary<Task<long>, SnakeAction>();
            for (int i = 0; i < 4; i++)
            {
                SnakeAction action = (SnakeAction)i;
                var newBoard = new GameBoard(gameBoard);

                if (PossiblyFilter.IsMovePossible(newBoard, action, lastMove, true, out Element newHead))
                {
                    tasks.Add(GetActionWeight(newBoard, newHead, action, 0), action);
                }
            }

            if (tasks.Count == 0)
                return null;

            bool first = true;
            SnakeAction result = SnakeAction.Down;
            long score = 0;

            do
            {
                var completedTask = await Task.WhenAny(tasks.Keys);

                var action = tasks[completedTask];
                long weight;
                if (completedTask.IsFaulted)
                {
                    weight = long.MinValue;
#if DEBUG
                    Graphical.WriteToLog(gameBoard);
#endif
                }
                else
                    weight = completedTask.Result;

                if (Helpers.IsDirectionBaff(action, ref gameBoard.Head, ref gameBoard.Tail))
                    weight += 1;

                if (Helpers.IsNearNyamkaBaff(action, ref gameBoard.Head, ref gameBoard.Nyamka))
                    weight += 10;

                if (first || score < weight)
                {
                    first = false;
                    result = action;
                    score = weight;
                }

                tasks.Remove(completedTask);
            } while (tasks.Count > 0);

            return result;
        }

        // Call recursively
        private static async Task<long> GetActionWeight(GameBoard gameBoard, Element newHead, SnakeAction action, int depth)
        {
            long currentRate = Rates.GetElementRate(gameBoard, ref newHead);
            if (currentRate == long.MinValue)
                return long.MinValue;

            //investigate new position
            //if (gameBoard.EvilTicks == 0)
            //{
            //    var (_, _, isEvilNear) = MeDetector.GetMe(gameBoard, gameBoard.Head.X, gameBoard.Head.Y);
            //    if (isEvilNear)
            //        currentRate -= 10;
            //}            

            currentRate *= (15 - depth);

            if (depth == maxDeep)
                return currentRate;

            //move me
#if DEBUG
            try
            {
#endif
                Movement.MakeMyMove(gameBoard, action, ref newHead);

                //add enemy predictions

#if DEBUG
            }
            catch (Exception)
            {
                Graphical.WriteToLog(gameBoard);
                throw;
            }
#endif

            Movement.MakeEnemyMove(gameBoard);

            //calculate my variants
            var tasks = new List<Task<long>>();
            for (int i = 0; i < 4; i++)
            {
                SnakeAction a = (SnakeAction)i;
                var newBoard = new GameBoard(gameBoard);                

                if (PossiblyFilter.IsMovePossible(newBoard, a, action, false, out var newNewHead))
                {
                    tasks.Add(GetActionWeight(newBoard, newNewHead, a, depth + 1));
                }
            }

            if (tasks.Count == 0)
                return long.MinValue;

            var myVariantsScore = long.MinValue;
            do
            {
                var completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);
#if DEBUG
                if (completedTask.IsFaulted)
                {
                    Graphical.WriteToLog(gameBoard);
                    throw completedTask.Exception;
                }
#endif
                if (!completedTask.IsFaulted && completedTask.Result != long.MinValue && myVariantsScore < completedTask.Result)
                {
                    myVariantsScore = completedTask.Result;
                }
            } while (tasks.Count > 0);

            if (myVariantsScore == long.MinValue)
                return long.MinValue;

            currentRate += myVariantsScore;

            return currentRate;
        }
    }
}
