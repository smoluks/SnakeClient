using System;

namespace SnakeBattle.Api
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public class GameBoard
    {
        public int Size;

        public BoardElement[,] Board;

        public Element Head;

        public BoardElement HeadType;

        public Element Tail;

        public BoardElement TailType;

        public int EvilTicks = 0;

        public bool HasStone = false;

        public int MyLength = 2;

        public Element Nyamka;

        public GameBoard(string boardString)
        {
            Head.X = -1;
            Tail.X = -1;

            boardString = boardString.Replace("\r", "").Replace("\n", "").Trim().ToLower();

            Size = (int)Math.Sqrt(boardString.Length);
            Board = new BoardElement[Size, Size];

            var chars = boardString.ToCharArray();

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    var element = (BoardElement)chars[x + y * Size];
                    if (Lists.IsHead(element))
                    {
                        Head.X = x;
                        Head.Y = y;
                        HeadType = element;
                    }
                    if (Lists.IsTail(element))
                    {
                        Tail.X = x;
                        Tail.Y = y;
                        TailType = element;
                    }
                    if (Lists.IsBody(element))
                    {
                        MyLength++;
                    }

                    Board[x, y] = element;
                }
            }

            if (Head.X != -1)
            {
                Element currentNyamka = new Element(-1, -1);
                float currentNyamkaLength = 9999;

                for (int y = 0; y < Size; y++)
                {
                    for (int x = 0; x < Size; x++)
                    {
                        if(Lists.IsNyamka(Board[x, y]) && (Math.Abs(Head.X - x)+ Math.Abs(Head.Y - y)) < currentNyamkaLength)
                        {
                            currentNyamka.X = x;
                            currentNyamka.Y = y;
                            currentNyamkaLength = Math.Abs(Head.X - x) + Math.Abs(Head.Y - y);
                        }
                    }
                }

                Nyamka = currentNyamka;                
            }

            //Console.WriteLine($"Head {Head.X}:{Head.Y}");
        }

        public GameBoard(GameBoard board)
        {
            Size = board.Size;

            Board = new BoardElement[Size, Size];
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    Board[x, y] = board.Board[x, y];
                }
            }

            Head = board.Head;

            HeadType = board.HeadType;

            Tail = board.Tail;

            TailType = board.TailType;

            EvilTicks = board.EvilTicks;

            HasStone = board.HasStone;

            MyLength = board.MyLength;

            Nyamka = board.Nyamka;
        }
    }
}
