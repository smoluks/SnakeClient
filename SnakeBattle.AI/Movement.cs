using SnakeBattle.Api;
using System;
using System.Collections.Generic;

namespace SnakeBattle.AI
{
    public static class Movement
    {

        public static void MakeMyMove(GameBoard gameBoard, SnakeAction action, ref Element newHead)
        {
            //Console.Clear();
            //Graphical.WriteField(gameBoard);

            //-----rage-----
            if (newHead.type == BoardElement.FuryPill)
                gameBoard.EvilTicks += 9;

            //clean enemies
            EnemyEraser.CleanEnemyShake(gameBoard, newHead.X, newHead.Y);
            var dontMoveTail = MeEraser.CleanMe(gameBoard, newHead.X, newHead.Y);

            MoveMyHead(gameBoard, ref newHead);

            if (!dontMoveTail && newHead.type != BoardElement.Apple)
            {
                MoveMyTail(gameBoard, ref newHead, false);
            }
            if (newHead.type == BoardElement.Apple)
            {
                gameBoard.MyLength++;
            }
            if (newHead.type == BoardElement.Stone)
            {
                gameBoard.HasStone = true;
                if (gameBoard.EvilTicks == 0)
                {
                    MoveMyTail(gameBoard, ref newHead, false);
                    MoveMyTail(gameBoard, ref newHead, false);
                    MoveMyTail(gameBoard, ref newHead, false);
                    gameBoard.MyLength -= 3;
                }
            }

            if (gameBoard.EvilTicks != 0)
                gameBoard.EvilTicks--;
        }

        private static void MoveMyTail(GameBoard gameBoard, ref Element newHead, bool putStone)
        {
            //clean current
            if (!Lists.IsHead(gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y]))
            {
                if (putStone)
                {
                    gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = BoardElement.Stone;
                    gameBoard.HasStone = false;
                }
                else gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = BoardElement.None;
            }

            //---calc coordinates---
            switch (gameBoard.TailType)
            {
                case BoardElement.TailEndDown:
                    gameBoard.Tail.Y--;
                    break;
                case BoardElement.TailEndLeft:
                    gameBoard.Tail.X++;
                    break;
                case BoardElement.TailEndRight:
                    gameBoard.Tail.X--;
                    break;
                case BoardElement.TailEndUp:
                    gameBoard.Tail.Y++;
                    break;
                default:
                    throw new SnakeException($"Bad tail move");
            }

            if (gameBoard.Tail.X < 0 || gameBoard.Tail.X > gameBoard.Size || gameBoard.Tail.Y < 0 || gameBoard.Tail.Y > gameBoard.Size)
                throw new SnakeException($"Bad tail at {gameBoard.Tail.X}:{gameBoard.Tail.Y}", gameBoard);

            var currentType = gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y];
            switch (currentType)
            {
                case BoardElement.BodyHorizontal:
                    switch (gameBoard.TailType)
                    {
                        case BoardElement.TailEndLeft:
                            gameBoard.TailType = BoardElement.TailEndLeft;
                            break;
                        case BoardElement.TailEndRight:
                            gameBoard.TailType = BoardElement.TailEndRight;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.BodyVertical:
                    switch (gameBoard.TailType)
                    {
                        case BoardElement.TailEndUp:
                            gameBoard.TailType = BoardElement.TailEndUp;
                            break;
                        case BoardElement.TailEndDown:
                            gameBoard.TailType = BoardElement.TailEndDown;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.BodyLeftUp:
                    switch (gameBoard.TailType)
                    {
                        case BoardElement.TailEndUp:
                            gameBoard.TailType = BoardElement.TailEndRight;
                            break;
                        case BoardElement.TailEndLeft:
                            gameBoard.TailType = BoardElement.TailEndDown;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.BodyRightUp:
                    switch (gameBoard.TailType)
                    {
                        case BoardElement.TailEndUp:
                            gameBoard.TailType = BoardElement.TailEndLeft;
                            break;
                        case BoardElement.TailEndRight:
                            gameBoard.TailType = BoardElement.TailEndDown;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.BodyLeftDown:
                    switch (gameBoard.TailType)
                    {
                        case BoardElement.TailEndDown:
                            gameBoard.TailType = BoardElement.TailEndRight;
                            break;
                        case BoardElement.TailEndLeft:
                            gameBoard.TailType = BoardElement.TailEndUp;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.BodyRightDown:
                    switch (gameBoard.TailType)
                    {
                        case BoardElement.TailEndDown:
                            gameBoard.TailType = BoardElement.TailEndLeft;
                            break;
                        case BoardElement.TailEndRight:
                            gameBoard.TailType = BoardElement.TailEndUp;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                default:
                    throw new SnakeException($"Bad body at {gameBoard.Tail.X}:{gameBoard.Tail.Y}", gameBoard);
            }

            //set fields
            gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = gameBoard.TailType;
        }

        private static void MoveMyHead(GameBoard gameBoard, ref Element newHead)
        {
            //find neck
            SnakeAction neckDirection;
            Element neck = new Element(gameBoard.Head.X, gameBoard.Head.Y);
            switch (gameBoard.HeadType)
            {
                case BoardElement.HeadDown:
                    neck.Y--;
                    break;
                case BoardElement.HeadUp:
                    neck.Y++;
                    break;
                case BoardElement.HeadLeft:
                    neck.X++;
                    break;
                case BoardElement.HeadRight:
                    neck.X--;
                    break;
            }

            //if (neck.X == -1)
            //    neck = Helpers.FindNear(gameBoard, gameBoard.Head.X, gameBoard.Head.Y, Lists.tails);
            if (neck.X - gameBoard.Head.X == 1)
            {
                //шея справа
                neckDirection = SnakeAction.Right;
            }
            else if (neck.X - gameBoard.Head.X == -1)
            {
                //шея слева
                neckDirection = SnakeAction.Left;
            }
            else if (neck.Y - gameBoard.Head.Y == 1)
            {
                //шея снизу
                neckDirection = SnakeAction.Down;
            }
            else if (neck.Y - gameBoard.Head.Y == -1)
            {
                //шея сверху
                neckDirection = SnakeAction.Up;
            }
            else
            {
                throw new SnakeException("Mutation detected");
            }

            //find head
            if (newHead.X - gameBoard.Head.X == 1)
            {
                //новая голова справа
                gameBoard.HeadType = BoardElement.HeadRight;
                gameBoard.Board[newHead.X, newHead.Y] = gameBoard.HeadType;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                          throw new SnakeException("Mutation detected");
                    case SnakeAction.Left:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyHorizontal;
                        break;
                    case SnakeAction.Down:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyRightDown;
                        break;
                    case SnakeAction.Up:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyRightUp;
                        break;
                }
            }
            else if (newHead.X - gameBoard.Head.X == -1)
            {
                //новая голова слева
                gameBoard.HeadType = BoardElement.HeadLeft;
                gameBoard.Board[newHead.X, newHead.Y] = gameBoard.HeadType;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyHorizontal;
                        break;
                    case SnakeAction.Left:
                        throw new SnakeException("Mutation detected");
                    case SnakeAction.Down:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyLeftDown;
                        break;
                    case SnakeAction.Up:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyLeftUp;
                        break;
                }
            }
            else if (newHead.Y - gameBoard.Head.Y == 1)
            {
                //новая голова снизу
                gameBoard.HeadType = BoardElement.HeadDown;
                gameBoard.Board[newHead.X, newHead.Y] = gameBoard.HeadType;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyRightDown;
                        break;
                    case SnakeAction.Left:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyLeftDown;
                        break;
                    case SnakeAction.Down:
                        throw new SnakeException("Mutation detected");
                    case SnakeAction.Up:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyVertical;
                        break;
                }
            }
            else if (newHead.Y - gameBoard.Head.Y == -1)
            {
                //новая голова сверху
                gameBoard.HeadType = BoardElement.HeadUp;
                gameBoard.Board[newHead.X, newHead.Y] = gameBoard.HeadType;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyRightUp;
                        break;
                    case SnakeAction.Left:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyLeftUp;
                        break;
                    case SnakeAction.Down:
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.BodyVertical;
                        break;
                    case SnakeAction.Up:
                        throw new SnakeException("Mutation detected");
                }
            }
            else
                throw new SnakeException("Mutation detected");

            gameBoard.Head.X = newHead.X;
            gameBoard.Head.Y = newHead.Y;
        }

        public static void MakeEnemyMove(GameBoard gameBoard)
        {
            //var xMin = gameBoard.Head.X < 10 ? 0 : gameBoard.Head.X - 10;
            //var xMax = gameBoard.Head.X + 10 >= gameBoard.Size ? gameBoard.Size - 1 : gameBoard.Head.X + 10;
            //var yMin = gameBoard.Head.Y < 10 ? 0 : gameBoard.Head.Y - 10;
            //var yMax = gameBoard.Head.Y + 10 >= gameBoard.Size ? gameBoard.Size - 1 : gameBoard.Head.Y + 10;
            var xMin = 0;
            var xMax = gameBoard.Size - 1;
            var yMin = 0;
            var yMax = gameBoard.Size - 1;

            List<(int x, int y)> EnemyTails = new List<(int x, int y)>();
            List<(int x, int y)> EnemyHeads = new List<(int x, int y)>();

            for (int y = yMin; y <= yMax; y++)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    var element = gameBoard.Board[x, y];

                    //heads
                    if (Lists.IsEnemyHead(element))
                        EnemyHeads.Add((x, y));

                    //tails
                    if (Lists.IsEnemyTail(element))
                    {
                        EnemyTails.Add((x, y));
                    }
                }
            }

            foreach (var (x, y) in EnemyHeads)
            {
                MoveEnemyHead(gameBoard, x, y);
            }

            foreach (var (x, y) in EnemyTails)
            {
                MoveEnemyTail(gameBoard, x, y);
            }
        }

        private static void MoveEnemyHead(GameBoard gameBoard, int x, int y)
        {
            var newHead = Helpers.FindNear(gameBoard, x, y, BoardElement.FuryPill);
            if(newHead.X == -1)
               newHead = Helpers.FindNear(gameBoard, x, y, BoardElement.Gold);
            if (newHead.X == -1)
                newHead = Helpers.FindNear(gameBoard, x, y, BoardElement.Apple);
            if (newHead.X == -1)
                newHead = Helpers.FindNear(gameBoard, x, y, BoardElement.None);
            if (newHead.X == -1)
                return;

            //find neck
            var headType = gameBoard.Board[x, y];
            bool aggro = false;

            SnakeAction neckDirection;
            Element neck = new Element(x, y);
            switch (headType)
            {
                case BoardElement.EnemyHeadDown:
                    neck.Y--;
                    break;
                case BoardElement.EnemyHeadUp:
                    neck.Y++;
                    break;
                case BoardElement.EnemyHeadLeft:
                    neck.X++;
                    break;
                case BoardElement.EnemyHeadRight:
                    neck.X--;
                    break;
                case BoardElement.EnemyHeadEvil:
                    aggro = true;
                    neck = Helpers.FindContinueEvilBody(gameBoard, x, y);
                    break;
                case BoardElement.EnemyHeadDead:
                    return;
            }

            if (neck.X - x == 1)
            {
                //шея справа
                neckDirection = SnakeAction.Right;
            }
            else if (neck.X - x == -1)
            {
                //шея слева
                neckDirection = SnakeAction.Left;
            }
            else if (neck.Y - y == 1)
            {
                //шея снизу
                neckDirection = SnakeAction.Down;
            }
            else if (neck.Y - y == -1)
            {
                //шея сверху
                neckDirection = SnakeAction.Up;
            }
            else
            {
                throw new SnakeException("Mutation detected");
            }

            //find head
            if (newHead.X - x == 1)
            {
                //новая голова справа
                gameBoard.Board[newHead.X, newHead.Y] = aggro ? BoardElement.EnemyHeadEvil : BoardElement.EnemyHeadRight;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                        throw new SnakeException("Mutation detected");
                    case SnakeAction.Left:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyHorizontal;
                        break;
                    case SnakeAction.Down:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyRightDown;
                        break;
                    case SnakeAction.Up:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyRightUp;
                        break;
                }
            }
            else if (newHead.X - x == -1)
            {
                //новая голова слева
                 gameBoard.Board[newHead.X, newHead.Y] = aggro ? BoardElement.EnemyHeadEvil : BoardElement.EnemyHeadLeft;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyHorizontal;
                        break;
                    case SnakeAction.Left:
                        throw new SnakeException("Mutation detected");
                    case SnakeAction.Down:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyLeftDown;
                        break;
                    case SnakeAction.Up:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyLeftUp;
                        break;
                }
            }
            else if (newHead.Y - y == 1)
            {
                //новая голова снизу
                gameBoard.Board[newHead.X, newHead.Y] = aggro ? BoardElement.EnemyHeadEvil : BoardElement.EnemyHeadDown;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyRightDown;
                        break;
                    case SnakeAction.Left:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyLeftDown;
                        break;
                    case SnakeAction.Down:
                        throw new SnakeException("Mutation detected");
                    case SnakeAction.Up:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyVertical;
                        break;
                }
            }
            else if (newHead.Y - y == -1)
            {
                //новая голова сверху
                gameBoard.Board[newHead.X, newHead.Y] = aggro ? BoardElement.EnemyHeadEvil : BoardElement.EnemyHeadUp;
                switch (neckDirection)
                {
                    case SnakeAction.Right:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyRightUp;
                        break;
                    case SnakeAction.Left:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyLeftUp;
                        break;
                    case SnakeAction.Down:
                        gameBoard.Board[x, y] = BoardElement.EnemyBodyVertical;
                        break;
                    case SnakeAction.Up:
                        throw new SnakeException("Mutation detected");
                }
            }
            else
                throw new SnakeException("Mutation detected");
        }

        private static void MoveEnemyTail(GameBoard gameBoard, int x, int y)
        {
            //clean current
            var tailType = gameBoard.Board[x, y];
            gameBoard.Board[x, y] = BoardElement.None;

            //---calc coordinates---
            switch (tailType)
            {
                case BoardElement.EnemyTailEndDown:
                    y--;
                    break;
                case BoardElement.EnemyTailEndLeft:
                    x++;
                    break;
                case BoardElement.EnemyTailEndRight:
                    x--;
                    break;
                case BoardElement.EnemyTailEndUp:
                    y++;
                    break;
                case BoardElement.EnemyTailInactive:
                    return;
                default:
                    throw new SnakeException($"Bad tail move");
            }

            if (x < 0 || x > gameBoard.Size || y < 0 || y > gameBoard.Size)
                throw new SnakeException($"Bad enemy tail at {x}:{y}", gameBoard);

            var currentType = gameBoard.Board[x, y];
            BoardElement newType;
            switch (currentType)
            {
                case BoardElement.EnemyBodyHorizontal:
                    switch (tailType)
                    {
                        case BoardElement.EnemyTailEndLeft:
                            newType = BoardElement.EnemyTailEndLeft;
                            break;
                        case BoardElement.EnemyTailEndRight:
                            newType = BoardElement.EnemyTailEndRight;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.EnemyBodyVertical:
                    switch (tailType)
                    {
                        case BoardElement.EnemyTailEndUp:
                            newType = BoardElement.EnemyTailEndUp;
                            break;
                        case BoardElement.EnemyTailEndDown:
                            newType = BoardElement.EnemyTailEndDown;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.EnemyBodyLeftUp:
                    switch (tailType)
                    {
                        case BoardElement.EnemyTailEndUp:
                            newType = BoardElement.EnemyTailEndRight;
                            break;
                        case BoardElement.EnemyTailEndLeft:
                            newType = BoardElement.EnemyTailEndDown;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.EnemyBodyRightUp:
                    switch (tailType)
                    {
                        case BoardElement.EnemyTailEndUp:
                            newType = BoardElement.EnemyTailEndLeft;
                            break;
                        case BoardElement.EnemyTailEndRight:
                            newType = BoardElement.EnemyTailEndDown;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.EnemyBodyLeftDown:
                    switch (tailType)
                    {
                        case BoardElement.EnemyTailEndDown:
                            newType = BoardElement.EnemyTailEndRight;
                            break;
                        case BoardElement.EnemyTailEndLeft:
                            newType = BoardElement.EnemyTailEndUp;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.EnemyBodyRightDown:
                    switch (tailType)
                    {
                        case BoardElement.EnemyTailEndDown:
                            newType = BoardElement.EnemyTailEndLeft;
                            break;
                        case BoardElement.EnemyTailEndRight:
                            newType = BoardElement.EnemyTailEndUp;
                            break;
                        default:
                            throw new SnakeException($"Bad tail move");
                    }
                    break;
                case BoardElement.EnemyHeadDown:
                case BoardElement.EnemyHeadUp:
                case BoardElement.EnemyHeadLeft:
                case BoardElement.EnemyHeadRight:
                case BoardElement.EnemyHeadEvil:
                case BoardElement.EnemyHeadDead:
                    gameBoard.Board[x, y] = BoardElement.None;
                    return;
                default:
                    return;
                    //throw new SnakeException($"Bad tail move: {x}:{y}", gameBoard);
            }

            //set fields
            gameBoard.Board[x, y] = newType;
        }
    }
}
