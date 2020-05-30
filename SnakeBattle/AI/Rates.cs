using SnakeBattle.Api;

namespace Client.AI
{
    internal static class Rates
    {
        internal static long GetElementRate(GameBoard gameBoard, ref Element newHead)
        {
            switch (newHead.type)
            {
                case BoardElement.None:
                    return 1;
                case BoardElement.Stone:
                    return gameBoard.EvilTicks > 0 ? 15 : (gameBoard.MyLength > 2 ? -100 : -1000);
                case BoardElement.Apple:
                    return 2;
                case BoardElement.Gold:
                    return 10;
                case BoardElement.FuryPill:
                    return 5;
                case BoardElement.EnemyHeadDown:
                case BoardElement.EnemyHeadLeft:
                case BoardElement.EnemyHeadRight:
                case BoardElement.EnemyHeadUp:
                    var (length, rage) = Helpers.GetEvilShake(newHead.X, newHead.Y, gameBoard);
                    if (length == -1)
                    {
                        return long.MinValue;
                    }

                    if (gameBoard.EvilTicks > 0)
                    {
                        if (!rage)
                            return 20;
                        else if (gameBoard.MyLength - length >= 2)
                            return 15;
                        else return -1;
                    }
                    else
                    {
                        if (rage)
                            return long.MinValue;
                        else if (gameBoard.MyLength - length >= 2)
                            return 20;
                        else return -1;
                    }

                case BoardElement.EnemyTailEndDown:
                case BoardElement.EnemyTailEndLeft:
                case BoardElement.EnemyTailEndUp:
                case BoardElement.EnemyTailEndRight:
                case BoardElement.EnemyTailInactive:
                case BoardElement.EnemyBodyHorizontal:
                case BoardElement.EnemyBodyVertical:
                case BoardElement.EnemyBodyLeftDown:
                case BoardElement.EnemyBodyLeftUp:
                case BoardElement.EnemyBodyRightDown:
                case BoardElement.EnemyBodyRightUp:
                    (length, rage) = Helpers.GetEvilShake(newHead.X, newHead.Y, gameBoard);
                    if (rage)
                        return gameBoard.EvilTicks > 0 ? 20 : -15;
                    else
                        return gameBoard.EvilTicks > 0 ? 20 : -1;
                //case BoardElement.HeadDown:
                //case BoardElement.HeadLeft:
                //case BoardElement.HeadRight:
                //case BoardElement.HeadUp:
                case BoardElement.TailEndDown:
                case BoardElement.TailEndLeft:
                case BoardElement.TailEndUp:
                case BoardElement.TailEndRight:
                case BoardElement.TailInactive:
                case BoardElement.BodyHorizontal:
                case BoardElement.BodyVertical:
                case BoardElement.BodyLeftDown:
                case BoardElement.BodyLeftUp:
                case BoardElement.BodyRightDown:
                case BoardElement.BodyRightUp:
                    return gameBoard.MyLength <= 2 ? long.MinValue : -100;
                default:
                    return long.MinValue;
            }
        }
    }
}
