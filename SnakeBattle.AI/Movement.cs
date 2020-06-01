using SnakeBattle.Api;
using System;

namespace SnakeBattle.AI
{
    public static class Movement
    {

        public static readonly BoardElement[] possiblyMove = new BoardElement[] {
            BoardElement.FuryPill,
            BoardElement.Gold,
            BoardElement.Apple,
            BoardElement.None
         };

        public static readonly BoardElement[] possiblyMoveWithEvil = new BoardElement[] {
            BoardElement.FuryPill,
            BoardElement.Stone,
            BoardElement.Gold,
            BoardElement.Apple,
            BoardElement.None
         };

        public static void MakeMyMove(GameBoard gameBoard, SnakeAction action, ref Element newHead)
        {
            //-----rage-----
            if (newHead.type == BoardElement.FuryPill)
                gameBoard.EvilTicks += 9;
            else if (gameBoard.EvilTicks != 0)
                gameBoard.EvilTicks--;

            //-----stone-----
            if (newHead.type == BoardElement.Stone)
            {
                gameBoard.HasStone = true;
                if (gameBoard.EvilTicks == 0)
                    gameBoard.MyLength -= 3;
            }

            //-----head-----
            //clean enemies
            EnemyEraser.CleanEnemyShake(gameBoard, newHead.X, newHead.Y);

            //find neck
            SnakeAction neckDirection;
            var neck = Helpers.FindNear(gameBoard, gameBoard.Head.X, gameBoard.Head.Y, Lists.bodies);
            if(neck.X == -1)
                neck = Helpers.FindNear(gameBoard, gameBoard.Head.X, gameBoard.Head.Y, Lists.tails);
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
            else throw new Exception("Mutation detected");

            //find head
            if (newHead.X - gameBoard.Head.X == 1)
            {
                //новая голова справа
                gameBoard.HeadType = BoardElement.HeadRight;
                gameBoard.Board[newHead.X, newHead.Y] = gameBoard.HeadType;
                switch(neckDirection)
                {
                    case SnakeAction.Right:
                        throw new Exception("Mutation detected");
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
                        throw new Exception("Mutation detected");
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
                        throw new Exception("Mutation detected");
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
                        throw new Exception("Mutation detected");
                }
            }
            else throw new Exception("Mutation detected");

            gameBoard.Head.X = newHead.X;
            gameBoard.Head.Y = newHead.Y;

            //-----tail-----
            if (newHead.type != BoardElement.Apple)
            {

                //---set type---
                if (gameBoard.Tail.X != 0 && Lists.IsBody(gameBoard.Board[gameBoard.Tail.X - 1, gameBoard.Tail.Y]))
                    gameBoard.TailType = BoardElement.TailEndRight;
                else if (gameBoard.Tail.X != gameBoard.Size - 1 && Lists.IsBody(gameBoard.Board[gameBoard.Tail.X + 1, gameBoard.Tail.Y]))
                    gameBoard.TailType = BoardElement.TailEndLeft;
                else if (gameBoard.Tail.Y != 0 && Lists.IsBody(gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y - 1]))
                    gameBoard.TailType = BoardElement.TailEndDown;
                else if (gameBoard.Tail.Y != gameBoard.Size - 1 && Lists.IsBody(gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y + 1]))
                    gameBoard.TailType = BoardElement.TailEndUp;

                else if (gameBoard.Tail.X != 0 && Lists.IsHead(gameBoard.Board[gameBoard.Tail.X - 1, gameBoard.Tail.Y]))
                    gameBoard.TailType = BoardElement.TailEndRight;
                else if (gameBoard.Tail.X != gameBoard.Size - 1 && Lists.IsHead(gameBoard.Board[gameBoard.Tail.X + 1, gameBoard.Tail.Y]))
                    gameBoard.TailType = BoardElement.TailEndLeft;
                else if (gameBoard.Tail.Y != 0 && Lists.IsHead(gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y - 1]))
                    gameBoard.TailType = BoardElement.TailEndDown;
                else if (gameBoard.Tail.Y != gameBoard.Size - 1 && Lists.IsHead(gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y + 1]))
                    gameBoard.TailType = BoardElement.TailEndUp;

                else throw new Exception("Mutation detected");

                //clean current
                if ((int)action >= 0x10 && gameBoard.HasStone)
                {
                    gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = BoardElement.Stone;
                    gameBoard.HasStone = false;
                }
                else gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = BoardElement.None;

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
                        throw new Exception($"Bad tail move");
                }

                //set fields
                gameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = gameBoard.TailType;
            }
        }

        public static void MakeEnemyMove(GameBoard state)
        {

            for (int y = 0; y < state.Size; y++)
            {
                for (int x = 0; x < state.Size; x++)
                {
                    var element = state.Board[x, y];

                    //heads
                    //if (Lists.IsEnemyHead(element))
                    //{
                    //    var els = element == BoardElement.EnemyHeadEvil ? possiblyMoveWithEvil : possiblyMove;

                    //    for (int i = 0; i < els.Length; i++)
                    //    {
                    //        var e = els[i];

                    //        var possibleHeads = Helpers.FindAllNear(state, x, y, e);
                    //        if (possibleHeads.Count > 0)
                    //        {
                    //            foreach (var possibleHead in possibleHeads)
                    //            {
                    //                var current = state.Board[possibleHead.X, possibleHead.Y];

                    //                state.Board[x, y] = BoardElement.EnemyBodyAbstract;
                    //                state.Board[possibleHead.X, possibleHead.Y] = current == BoardElement.FuryPill ? BoardElement.EnemyHeadEvil : element;

                    //            }
                    //            break;
                    //        }
                    //    }
                    //}

                    //tails
                    if (Lists.IsEnemyTail(element))
                    {
                        state.Board[x, y] = BoardElement.None;

                        var body = FindNearEvilBodies(state, x, y);
                        if (body.X != -1)
                            state.Board[body.X, body.Y] = BoardElement.EnemyTailInactive;
                    }
                }
            }
        }

        internal static Element FindNearEvilBodies(GameBoard board, int x, int y)
        {
            if (x != 0 && Lists.IsEnemyBody(board.Board[x - 1, y]))
                return new Element(x - 1, y);

            if (x != board.Size - 1 && Lists.IsEnemyBody(board.Board[x + 1, y]))
                return new Element(x + 1, y);

            if (y != 0 && Lists.IsEnemyBody(board.Board[x, y - 1]))
                return new Element(x, y - 1);

            if (y != board.Size - 1 && Lists.IsEnemyBody(board.Board[x, y + 1]))
                return new Element(x, y + 1);

            return new Element(-1, -1);
        }
    }
}
