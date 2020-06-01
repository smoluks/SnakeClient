using SnakeBattle.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeBattle.AI
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

        internal static Element FindNear(GameBoard gameBoard, int x, int y, BoardElement[] elements)
        {
            if (x != 0 && elements.Contains(gameBoard.Board[x - 1, y]))
                return new Element(x - 1, y);

            if (x != gameBoard.Size - 1 && elements.Contains(gameBoard.Board[x + 1, y]))
                return new Element(x + 1, y);

            if (y != 0 && elements.Contains(gameBoard.Board[x, y - 1]))
                return new Element(x, y - 1);

            if (y != gameBoard.Size - 1 && elements.Contains(gameBoard.Board[x, y + 1]))
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

        internal static bool IsNear(GameBoard gameBoard, int x, int y, BoardElement[] elements)
        {
            if (x != 0 && elements.Contains(gameBoard.Board[x - 1, y]))
                return true;

            if (x != gameBoard.Size - 1 && elements.Contains(gameBoard.Board[x + 1, y]))
                return true;

            if (y != 0 && elements.Contains(gameBoard.Board[x, y - 1]))
                return true;

            if (y != gameBoard.Size - 1 && elements.Contains(gameBoard.Board[x, y + 1]))
                return true;

            return false;
        }

        internal static bool IsNear(GameBoard gameBoard, int x, int y, BoardElement element)
        {
            if (x != 0 && gameBoard.Board[x - 1, y] == element)
                return true;

            if (x != gameBoard.Size - 1 && gameBoard.Board[x - 1, y] == element)
                return true;

            if (y != 0 && gameBoard.Board[x - 1, y] == element)
                return true;

            if (y != gameBoard.Size - 1 && gameBoard.Board[x - 1, y] == element)
                return true;

            return false;
        }
    }
}
