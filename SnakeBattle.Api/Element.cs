using System;

namespace SnakeBattle.Api
{
    public struct Element
    {
        public int X;

        public int Y;

        public BoardElement type;

        public Element(int x, int y)
        {
            X = x;
            Y = y;
            type = BoardElement.None;
        }

        public Element MakeMovement(SnakeAction action, int boardSize)
        {
            int x = X;
            int y = Y;

            switch ((int)action & 0x0F)
            {
                case (int)SnakeAction.Down:
                    y++;
                    break;
                case (int)SnakeAction.Up:
                    y--;
                    break;
                case (int)SnakeAction.Left:
                    x--;
                    break;
                case (int)SnakeAction.Right:
                    x++;
                    break;
            }

            if (x < 0 || x >= boardSize || y < 0 || y >= boardSize)
                return new Element(-1, -1);

            return new Element(x, y);
        }

        /// <summary>
        /// Checks is current point on board or out of range.
        /// </summary>
        /// <param name="boardSize">Board size to compare</param>
        public bool IsOutOfBoard(int boardSize)
        {
            return X >= boardSize || Y >= boardSize || X < 0 || Y < 0;
        }

        /// <summary>
        /// Returns new BoardPoint object shifted left to "delta" points
        /// </summary>
        public Element ShiftLeft(int delta = 1)
        {
            return new Element((int)(this.X - delta), (int)this.Y);
        }

        /// <summary>
        /// Returns new BoardPoint object shifted right to "delta" points
        /// </summary>
        public Element ShiftRight(int delta = 1)
        {
            return new Element((int)(this.X + delta), (int)this.Y);
        }

        /// <summary>
        /// Returns new BoardPoint object shifted top "delta" points
        /// </summary>
        public Element ShiftTop(int delta = 1)
        {
            return new Element((int)this.X, (int)(this.Y - delta));
        }

        /// <summary>
        /// Returns new BoardPoint object shifted bottom "delta" points
        /// </summary>
        public Element ShiftBottom(int delta = 1)
        {
            return new Element((int)this.X, (int)(this.Y + delta));

        }

        public static bool operator ==(Element p1, Element p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool operator !=(Element p1, Element p2)
        {
            return !(p1 == p2);
        }

        public override string ToString()
        {
            return $"[{X},{Y}]";
        }

        public override bool Equals(object obj)
        {
            return obj is Element boardPoint && boardPoint == this;
        }

        public override int GetHashCode()
        {
            const int mult = 17;
            unchecked
            {
                return (X.GetHashCode() * mult) ^ Y.GetHashCode();
            }
        }
    }
}
