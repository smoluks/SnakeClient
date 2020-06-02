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

        //Find body from evil head (no head)
        internal static Element FindContinueEvilBody(GameBoard gameBoard, int x, int y)
        {
            int newX;
            int newY;

            //left
            if (x != 0)
            {
                newX = x - 1;
                newY = y;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyHorizontal || newElement == BoardElement.EnemyBodyRightDown || newElement == BoardElement.EnemyBodyRightUp || newElement == BoardElement.EnemyTailEndLeft)
                    return new Element(newX, newY);
            }

            //right
            if (x != gameBoard.Size - 1)
            {
                newX = x + 1;
                newY = y;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyHorizontal || newElement == BoardElement.EnemyBodyLeftDown || newElement == BoardElement.EnemyBodyLeftUp || newElement == BoardElement.EnemyTailEndRight)
                    return new Element(newX, newY);
            }

            //up
            if (y != 0)
            {
                newX = x;
                newY = y - 1;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyVertical || newElement == BoardElement.EnemyBodyRightDown || newElement == BoardElement.EnemyBodyLeftDown || newElement == BoardElement.EnemyTailEndUp)
                    return new Element(newX, newY);
            }

            //down
            if (y != gameBoard.Size - 1)
            {
                newX = x;
                newY = y + 1;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyVertical || newElement == BoardElement.EnemyBodyRightUp || newElement == BoardElement.EnemyBodyLeftUp || newElement == BoardElement.EnemyTailEndDown)
                    return new Element(newX, newY);
            }

            return new Element(-1, -1);
        }

        //Find body from body (no tail)
        internal static Element FindEvilBody(GameBoard gameBoard, int x, int y)
        {
            int newX;
            int newY;

            //left
            if (x != 0)
            {
                newX = x - 1;
                newY = y;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyHorizontal || newElement == BoardElement.EnemyBodyRightDown || newElement == BoardElement.EnemyBodyRightUp || newElement == BoardElement.EnemyHeadLeft || newElement == BoardElement.EnemyHeadEvil)
                    return new Element(newX, newY);
            }

            //right
            if (x != gameBoard.Size - 1)
            {
                newX = x + 1;
                newY = y;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyHorizontal || newElement == BoardElement.EnemyBodyLeftDown || newElement == BoardElement.EnemyBodyLeftUp || newElement == BoardElement.EnemyHeadRight || newElement == BoardElement.EnemyHeadEvil)
                    return new Element(newX, newY);
            }

            //up
            if (y != 0)
            {
                newX = x;
                newY = y - 1;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyVertical || newElement == BoardElement.EnemyBodyRightDown || newElement == BoardElement.EnemyBodyLeftDown || newElement == BoardElement.EnemyHeadUp || newElement == BoardElement.EnemyHeadEvil)
                    return new Element(newX, newY);
            }

            //down
            if (y != gameBoard.Size - 1)
            {
                newX = x;
                newY = y + 1;
                var newElement = gameBoard.Board[newX, newY];
                if (newElement == BoardElement.EnemyBodyVertical || newElement == BoardElement.EnemyBodyRightUp || newElement == BoardElement.EnemyBodyLeftUp || newElement == BoardElement.EnemyHeadDown || newElement == BoardElement.EnemyHeadEvil)
                    return new Element(newX, newY);
            }

            return new Element(-1, -1);
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

        internal static Element FindMeBody(GameBoard gameBoard, int x, int y)
        {
            var current = gameBoard.Board[x, y];

            //left
            if (x != 0 && (current == BoardElement.BodyHorizontal || current == BoardElement.BodyLeftDown || current == BoardElement.BodyLeftUp || current == BoardElement.TailEndRight) && Lists.bodies.Contains(gameBoard.Board[x - 1, y]))
                return new Element(x - 1, y);

            //rigth
            if (x != gameBoard.Size - 1 && (current == BoardElement.BodyHorizontal || current == BoardElement.BodyRightDown || current == BoardElement.BodyRightUp || current == BoardElement.TailEndLeft) && Lists.bodies.Contains(gameBoard.Board[x + 1, y]))
                return new Element(x + 1, y);

            //up
            if (y != 0 && (current == BoardElement.BodyVertical || current == BoardElement.BodyRightUp || current == BoardElement.BodyLeftUp || current == BoardElement.TailEndDown) && Lists.bodies.Contains(gameBoard.Board[x, y - 1]))
                return new Element(x, y - 1);

            //down
            if (y != gameBoard.Size - 1 && (current == BoardElement.BodyVertical || current == BoardElement.BodyRightDown || current == BoardElement.BodyLeftDown || current == BoardElement.TailEndUp) && Lists.bodies.Contains(gameBoard.Board[x, y + 1]))
                return new Element(x, y + 1);

            return new Element(-1, -1);
        }

        internal static Element FindMeBody(GameBoard gameBoard, int x, int y, BoardElement[] elements)
        {
            var current = gameBoard.Board[x, y];

            //left
            if (x != 0 && (current == BoardElement.BodyHorizontal || current == BoardElement.BodyLeftDown || current == BoardElement.BodyLeftUp || current == BoardElement.TailEndRight) && elements.Contains(gameBoard.Board[x - 1, y]))
                return new Element(x - 1, y);

            //rigth
            if (x != gameBoard.Size - 1 && (current == BoardElement.BodyHorizontal || current == BoardElement.BodyRightDown || current == BoardElement.BodyRightUp || current == BoardElement.TailEndLeft) && elements.Contains(gameBoard.Board[x + 1, y]))
                return new Element(x + 1, y);

            //up
            if (y != 0 && (current == BoardElement.BodyVertical || current == BoardElement.BodyRightUp || current == BoardElement.BodyLeftUp || current == BoardElement.TailEndDown) && elements.Contains(gameBoard.Board[x, y - 1]))
                return new Element(x, y - 1);

            //down
            if (y != gameBoard.Size - 1 && (current == BoardElement.BodyVertical || current == BoardElement.BodyRightDown || current == BoardElement.BodyLeftDown || current == BoardElement.TailEndUp) && elements.Contains(gameBoard.Board[x, y + 1]))
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

        internal static bool IsNear(GameBoard gameBoard, int x, int y, BoardElement enemyHeadEvil, int depth)
        {
            var xMin = x < depth ? 0 : x - depth;
            var xMax =x + depth >= gameBoard.Size ? gameBoard.Size - 1 : x + depth;
            var yMin = y < depth ? 0 : y - depth;
            var yMax = y + depth >= gameBoard.Size ? gameBoard.Size - 1 : y + depth;

            for(int i = xMin; i <= xMax; i++)
                for (int j = yMin; j <= yMax; j++)
                {
                    if (gameBoard.Board[i, j] == enemyHeadEvil)
                        return true;
                }

            return false;
        }
    }
}
