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
