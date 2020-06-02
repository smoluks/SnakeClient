using SnakeBattle.Api;
using System;

namespace Client
{
    static class StateContoller
    {
        internal static int rageCount = 0;
        internal static bool hasStone = false;
        internal static SnakeAction LastMove { get; set; }

        internal static void SetState(BoardElement eatedElement, SnakeAction action)
        {
            if (eatedElement == BoardElement.FuryPill)
                rageCount += 9;
            else if (rageCount > 0)
            {
                rageCount--;
            }

            if ((int)action >= 0x10)
            {
                hasStone = false;
            }

            if (eatedElement == BoardElement.Stone)
            {
                hasStone = true;
            }

            LastMove = action;
        }

        internal static void UpdateState(GameBoard gameBoard)
        {
            if(gameBoard.HeadType == BoardElement.HeadEvil)
            {
                //if (rageCount == 0)
                //    throw new Exception("Evil detect failed");

                switch(LastMove)
                {
                    case SnakeAction.Down:
                        gameBoard.HeadType = BoardElement.HeadDown;
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.HeadDown;
                        break;
                    case SnakeAction.Up:
                        gameBoard.HeadType = BoardElement.HeadUp;
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.HeadUp;
                        break;
                    case SnakeAction.Left:
                        gameBoard.HeadType = BoardElement.HeadLeft;
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.HeadLeft;
                        break;
                    case SnakeAction.Right:
                        gameBoard.HeadType = BoardElement.HeadRight;
                        gameBoard.Board[gameBoard.Head.X, gameBoard.Head.Y] = BoardElement.HeadRight;
                        break;
                }
            }
            gameBoard.EvilTicks = rageCount;
            gameBoard.HasStone = hasStone;
        }

        internal static void CleanState()
        {
            rageCount = 0;
            hasStone = false;
        }
    }
}
