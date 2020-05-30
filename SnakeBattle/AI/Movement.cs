using SnakeBattle.Api;
using System;

namespace Client.AI
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

        internal static GameBoard MakeMyMove(GameBoard gameBoard, SnakeAction action, ref Element newHead)
        {
            var newGameBoard = new GameBoard(gameBoard);

            //tail
            if (newHead.type != BoardElement.Apple)
            {
                Element newTail;
                switch (gameBoard.TailType)
                {
                    case BoardElement.TailEndDown:
                        newTail = gameBoard.Tail.MakeMovement(SnakeAction.Up, gameBoard.Size);
                        break;
                    case BoardElement.TailEndLeft:
                        newTail = gameBoard.Tail.MakeMovement(SnakeAction.Right, gameBoard.Size);
                        break;
                    case BoardElement.TailEndRight:
                        newTail = gameBoard.Tail.MakeMovement(SnakeAction.Left, gameBoard.Size);
                        break;
                    case BoardElement.TailEndUp:
                        newTail = gameBoard.Tail.MakeMovement(SnakeAction.Down, gameBoard.Size);
                        break;
                    default:
                        Console.WriteLine($"Bad tail move");
                        newTail = new Element(-1, -1);
                        break;
                }

                if (newTail.X != -1)
                {
                    newGameBoard.Board[newTail.X, newTail.Y] = BoardElement.TailEndDown;
                    newGameBoard.Tail = newTail;
                    if ((int)action >= 0x10 && gameBoard.HasStone)
                    {
                        newGameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = BoardElement.Stone;
                        newGameBoard.HasStone = false;
                    }
                    else newGameBoard.Board[gameBoard.Tail.X, gameBoard.Tail.Y] = BoardElement.None;
                }
            }

            //head
            newGameBoard.Head = newHead;

            switch ((SnakeAction)((int)action & 0x0F))
            {
                case SnakeAction.Down:
                    newGameBoard.Board[newHead.X, newHead.Y] = BoardElement.HeadDown;
                    newGameBoard.HeadType = BoardElement.HeadDown;
                    break;
                case SnakeAction.Up:
                    newGameBoard.Board[newHead.X, newHead.Y] = BoardElement.HeadUp;
                    newGameBoard.HeadType = BoardElement.HeadUp;
                    break;
                case SnakeAction.Left:
                    newGameBoard.Board[newHead.X, newHead.Y] = BoardElement.HeadLeft;
                    newGameBoard.HeadType = BoardElement.HeadLeft;
                    break;
                case SnakeAction.Right:
                    newGameBoard.Board[newHead.X, newHead.Y] = BoardElement.HeadRight;
                    newGameBoard.HeadType = BoardElement.HeadRight;
                    break;
            }

            //rage
            if (newHead.type == BoardElement.FuryPill)
                newGameBoard.EvilTicks = 14;
            else if (newGameBoard.EvilTicks != 0)
                newGameBoard.EvilTicks--;

            //stone
            if (newHead.type == BoardElement.Stone)
            {
                newGameBoard.HasStone = true;
                if (newGameBoard.EvilTicks == 0)
                    newGameBoard.MyLength -= 3;
            }

            return newGameBoard;
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
