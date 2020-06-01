using SnakeBattle.AI;
using SnakeBattle.Api;
using System;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            TestMyMovement();
        }

        private static void TestMyMovement()
        {
            var gameBoard = new GameBoard(Boards.realDiagonal);
            SnakeAction lastmove = SnakeAction.Right;

            while (true)
            {
                Console.Clear();
                Graphical.WriteField(gameBoard);
                Console.WriteLine($"{gameBoard.EvilTicks} {gameBoard.HasStone}");

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
