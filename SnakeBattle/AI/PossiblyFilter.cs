using SnakeBattle.Api;

namespace Client.AI
{
    static class PossiblyFilter
    {
        internal static bool IsMovePossible(GameBoard gameBoard, SnakeAction actionS, SnakeAction lastMoveS, bool real, out Element newHead)
        {

            var action = (int)actionS & 0x0F;
            var lastMove = (int)lastMoveS & 0x0F;

            newHead = new Element(-1, -1);

            //180 degrees
            switch (gameBoard.HeadType)
            {
                case BoardElement.HeadUp:
                    if (action == (int)SnakeAction.Down)
                        return false;
                    break;
                case BoardElement.HeadLeft:
                    if (action == (int)SnakeAction.Right)
                        return false;
                    break;
                case BoardElement.HeadRight:
                    if (action == (int)SnakeAction.Left)
                        return false;
                    break;
                case BoardElement.HeadDown:
                    if (action == (int)SnakeAction.Up)
                        return false;
                    break;
                case BoardElement.HeadEvil:
                    switch (lastMove & 0x0F)
                    {
                        case (int)SnakeAction.Up:
                            if (action == (int)SnakeAction.Down)
                                return false;
                            break;
                        case (int)SnakeAction.Left:
                            if (action == (int)SnakeAction.Right)
                                return false;
                            break;
                        case (int)SnakeAction.Right:
                            if (action == (int)SnakeAction.Left)
                                return false;
                            break;
                        case (int)SnakeAction.Down:
                            if (action == (int)SnakeAction.Up)
                                return false;
                            break;
                    }
                    break;
            }

            //over field
            newHead = gameBoard.Head.MakeMovement(actionS, gameBoard.Size);
            if (newHead.X == -1)
            {
                return false;
            }

            //stones
            newHead.type = gameBoard.Board[newHead.X, newHead.Y];

            //Console.WriteLine($"{action}: {newHead.X}:{newHead.Y} - {newHeadPoint}");

            if (newHead.type == BoardElement.StartFloor || newHead.type == BoardElement.Wall)
                return false;

            if (gameBoard.EvilTicks == 0 && gameBoard.MyLength < 3 && newHead.type == BoardElement.Stone)
                return false;

            //me
            if (gameBoard.MyLength <= 2 && (Lists.IsHead(newHead.type) || Lists.IsBody(newHead.type) || Lists.IsTail(newHead.type)))
                return false;

            //enemies
            if (real)
            {
                if (gameBoard.EvilTicks == 0)
                {
                    if (Lists.IsEnemyHead(newHead.type) || Lists.IsEnemyBody(newHead.type) || Lists.IsEnemyTail(newHead.type))
                        return false;
                }
                else
                {
                    if (newHead.type == BoardElement.EnemyHeadEvil)
                        return false;
                }
            }

            return true;
        }
    }
}
