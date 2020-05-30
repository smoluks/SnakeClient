using SnakeBattle.Api;
using System;
using System.Collections.Generic;

namespace Client.AI
{
    static class Helpers
    {     
        internal static bool IsDirectionBaff(SnakeAction action, ref Element head, ref Element tail)
        {
            switch ((SnakeAction)((int)action & 0x0F))
            {
                case SnakeAction.Up:
                    return tail.Y > head.Y;
                case SnakeAction.Down:
                    return tail.Y < head.Y;
                case SnakeAction.Left:
                    return tail.X > head.X;
                case SnakeAction.Right:
                    return tail.X < head.X;
            }

            return false;
        }

        internal static bool IsNearNyamkaBaff(SnakeAction action, ref Element head, ref Element nyamka)
        {
            switch ((SnakeAction)((int)action & 0x0F))
            {
                case SnakeAction.Up:
                    return nyamka.Y < head.Y;
                case SnakeAction.Down:
                    return nyamka.Y > head.Y;
                case SnakeAction.Left:
                    return nyamka.X < head.X;
                case SnakeAction.Right:
                    return nyamka.X > head.X;
            }

            return false;
        }

        internal static Element FindNear(GameBoard board, int x, int y, BoardElement element)
        {
            if (x != 0 && board.Board[x - 1, y] == element)
                return new Element(x - 1, y);

            if (x != board.Size - 1 && board.Board[x + 1, y] == element)
                return new Element(x + 1, y);

            if (y != 0 && board.Board[x, y - 1] == element)
                return new Element(x, y - 1);

            if (y != board.Size - 1 && board.Board[x, y + 1] == element)
                return new Element(x, y + 1);

            return new Element(-1, -1);
        }

        internal static List<Element> FindAllNear(GameBoard board, int x, int y, BoardElement element)
        {
            var result = new List<Element>();

            if (x != 0 && board.Board[x - 1, y] == element)
                result.Add(new Element(x - 1, y));

            if (x != board.Size - 1 && board.Board[x + 1, y] == element)
                result.Add(new Element(x + 1, y));

            if (y != 0 && board.Board[x, y - 1] == element)
                result.Add(new Element(x, y - 1));

            if (y != board.Size - 1 && board.Board[x, y + 1] == element)
                result.Add(new Element(x, y + 1));

            return result;
        }

        internal static (int length, bool rage) GetEvilShake(int x, int y, GameBoard gameBoard)
        {

            //var el = gameBoard.Board[newHead.X, newHead.Y];
            //if (!Lists.IsEnemyHead(el) && !Lists.IsEnemyBody(el) && !Lists.IsEnemyTail(el))
            //    return (-1, false);

            bool isEvil = false;
            var snakePart = new HashSet<int>();
            GetEvilShakeInternal(gameBoard, x, y, ref snakePart, ref isEvil);

            return (snakePart.Count, isEvil);
        }

        private static void GetEvilShakeInternal(GameBoard gameBoard, int checkPositionX, int checkPositionY, ref HashSet<int> snakePart, ref bool evil)
        {
            snakePart.Add((checkPositionY << 8) + checkPositionX);

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if(x == 0 && y == 0)
                        continue;

                    int newX = checkPositionX + x;
                    int newY = checkPositionY + y;

                    if (newX < 0 || newY < 0 || newX >= gameBoard.Size || newY >= gameBoard.Size)
                        continue;

                    var el = gameBoard.Board[newX, newY];
                    if (!Lists.IsEnemyHead(el) && !Lists.IsEnemyBody(el) && !Lists.IsEnemyTail(el))
                        continue;

                    if (snakePart.Contains((newY << 8) + newX))
                        continue;

                    if (Lists.IsEnemyHead(el))
                        evil = el == BoardElement.EnemyHeadEvil;

                    GetEvilShakeInternal(gameBoard, newX, newY, ref snakePart, ref evil);
                }
            }

            return;
        }

    }
}
