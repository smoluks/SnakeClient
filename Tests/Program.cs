using SnakeBattle.AI;
using SnakeBattle.Api;
using System;

namespace Tests
{
    class Program
    {
        public static SnakeAction LastMove { get; private set; }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            TestMyMovement();
        }

        private static void TestMyMovement()
        {
            var gameBoard = new GameBoard(Boards.Current);
            SnakeAction lastmove = SnakeAction.Left;

            while (true)
            {
                Movement.MakeEnemyMove(gameBoard);

                Console.Clear();
                Graphical.WriteField(gameBoard);
                Console.WriteLine($"{gameBoard.Head.X}:{gameBoard.Head.Y}:{gameBoard.HeadType}: {gameBoard.MyLength} {gameBoard.EvilTicks} {gameBoard.HasStone}");

                bool possible = false;
                SnakeAction action = SnakeAction.ActDown;
                Element newHead = new Element(-1, -1);
                do
                {
                    var key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            action = SnakeAction.Up;
                            break;
                        case ConsoleKey.DownArrow:
                            action = SnakeAction.Down;
                            break;
                        case ConsoleKey.LeftArrow:
                            action = SnakeAction.Left;
                            break;
                        case ConsoleKey.RightArrow:
                            action = SnakeAction.Right;
                            break;
                        default:
                            continue;
                    }

                    if (!PossiblyFilter.IsMovePossible(gameBoard, action, lastmove, true, out newHead))
                        Console.WriteLine("Impossible");
                    else possible = true;
                }
                while (!possible);

                lastmove = action;

                gameBoard = new GameBoard(gameBoard);
                Movement.MakeMyMove(gameBoard, action, ref newHead);                
            }
        }
    }
}
