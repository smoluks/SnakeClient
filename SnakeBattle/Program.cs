using SnakeBattle.AI;
using SnakeBattle.Api;
using SnakeBattle.Api.Clients;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        const string PATH = "http://codebattle-pro-2020s1.westeurope.cloudapp.azure.com/codenjoy-contest/board/player/uf808qnza8qd66wv6vr5?code=3433622376354117498&gameName=snakebattle";

        private static SnakeBattleHTTPClient client;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            int i = 1073741824;
            while (!ThreadPool.SetMinThreads(i, 1000))
            {
                i /= 2;
            }
            Console.WriteLine($"Max threads {i}");

            do
            {
                client = new SnakeBattleHTTPClient(PATH, StepHandler, TimeoutError);
            }
            while(!client.Connect());
            Console.WriteLine($"Connected");

            do
            {
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        client.Disconnect();
                        return;
                    case ConsoleKey.Add:
                        AIController.maxDeep++;
                        Console.WriteLine($"Set depth {AIController.maxDeep}");
                        break;
                    case ConsoleKey.Subtract:
                        AIController.maxDeep--;
                        Console.WriteLine($"Set depth {AIController.maxDeep}");
                        break;
                    case ConsoleKey.Enter:
                        client.save = true;
                        break;
                }
            }
            while (true);
        }

        private static void TimeoutError(long time)
        {
            AIController.maxDeep--;
            Console.WriteLine($"Depth autodecrement {AIController.maxDeep}");
        }

        static bool cacheIsClean = true;

        private static SnakeAction? StepHandler(GameBoard gameBoard)
        {
            
            if (gameBoard.Head.X == -1 || gameBoard.HeadType == BoardElement.HeadDead)
            {
                Console.WriteLine($"Waiting to start game");
                if (!cacheIsClean)
                {
                    GC.Collect();
                    StateContoller.CleanState();                    
                }

                cacheIsClean = true;
                return null;
            }

            cacheIsClean = false;

            StateContoller.UpdateState(gameBoard);

            var result = AIController.GetOptimalActionAsync(gameBoard, StateContoller.LastMove).GetAwaiter().GetResult();

            Task.Run(() =>
            {
                if (result.HasValue)
                {
                    var newHead = gameBoard.Head.MakeMovement(result.Value, gameBoard.Size);

                    StateContoller.SetState(gameBoard.Board[newHead.X, newHead.Y], result.Value);
                }
            });

            return result;
        }
    }
}