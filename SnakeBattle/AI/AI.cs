using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.AI
{
    internal class AIController
    {
        internal static int maxDeep = 10;
        public static async Task<SnakeAction?> GetOptimalActionAsync(GameBoard gameBoard, SnakeAction lastMove)
        {
            var tasks = new Dictionary<Task<long>, SnakeAction>();
            for (int i = 0; i < 4; i++)
            {
                SnakeAction action = (SnakeAction)i;

                if (PossiblyFilter.IsMovePossible(gameBoard, action, lastMove, true, out Element newHead))
                {
                    tasks.Add(GetActionWeight(gameBoard, newHead, action, 0), action);
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
                var weight = completedTask.Result;

#if (DEBUG)
                Console.WriteLine($"{action} {weight}");
#endif

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
            //investigate new position
            var currentRate = Rates.GetElementRate(gameBoard, ref newHead) * (20 - depth);
            if (currentRate == long.MinValue)
                return long.MinValue;

            if (depth == maxDeep)
                return currentRate;

            //move me
            var newGameBoard = Movement.MakeMyMove(gameBoard, action, ref newHead);

            //add enemy predictions
            Movement.MakeEnemyMove(newGameBoard);

            //calculate my variants
            var tasks = new List<Task<long>>();
            for (int i = 0; i < 4; i++)
            {
                SnakeAction a = (SnakeAction)i;

                if (PossiblyFilter.IsMovePossible(newGameBoard, a, action, false, out var newNewHead))
                    tasks.Add(GetActionWeight(newGameBoard, newNewHead, a, depth + 1));
            }

            if (tasks.Count == 0)
                return long.MinValue;

            var myVariantsScore = long.MinValue;
            do
            {
                var completedTask = await Task.WhenAny(tasks);
                tasks.Remove(completedTask);

                var weight = completedTask.Result;
                if (weight != long.MinValue && myVariantsScore < weight)
                {
                    myVariantsScore = weight;
                }
            } while (tasks.Count > 0);

            if (myVariantsScore == long.MinValue)
                return long.MinValue;

            currentRate += myVariantsScore;

            return currentRate;
        }        
    }
}
