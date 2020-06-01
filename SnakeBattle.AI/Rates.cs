using SnakeBattle.Api;
using System;

namespace SnakeBattle.AI
{
    internal static class Rates
    {
        internal static long GetElementRate(GameBoard gameBoard, ref Element newHead)
        {
            //---tails---
            if (Lists.IsEnemyHead(newHead.type) || (Lists.IsEnemyBody(newHead.type) && Helpers.IsNear(gameBoard, newHead.X, newHead.Y, Lists.enemyHeads)))
            {
                //---head---
                var enemyAgro = newHead.type == BoardElement.EnemyHeadEvil || Helpers.IsNear(gameBoard, newHead.X, newHead.Y, BoardElement.EnemyHeadEvil);

                //--правила для головы--
                //Если текущая змейка в ярости, а вражеская нет - то убиваем вражескую змейку. 
                if (gameBoard.EvilTicks> 0 && !enemyAgro)
                    return 1000;

                //Если текущая змейка не в ярости, а вражеская в ярости - то текущая змейка умирает.
                else if (gameBoard.EvilTicks == 0 && enemyAgro)
                    throw new Exception("Do it in filter!");

                //Если обе змейки в ярости или обе не в ярости - то от каждой отрезаем длину другой змейки. 
                //То есть, если столкнулись две змейки длиной в 5 и в 7 единиц, то змейка длиной в пять погибает, так как 5-7<2, а змейка длиной в 7 принимает длину 2 единицы.
                else if ((gameBoard.EvilTicks != 0) == enemyAgro)
                {
                    var enemy = EnemyDetector.GetEnemyShake(gameBoard, newHead.X, newHead.Y);

                    if (gameBoard.MyLength - (enemy.headLength + enemy.tailLength + 1) < 2)
                        throw new Exception("Do it in filter!"); 

                    else return (enemy.headLength + enemy.tailLength + 1) * 10;
                }
            }

            switch (newHead.type)
            {
                //---enemy---
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
                    //Если таковые имеются, то тогда проверяем, а текущая змейка находится под таблеткой ярости или нет:
                    if (gameBoard.EvilTicks > 0)
                    {
                        //4.1.Если да - тогда от вражеской змейки отгрызаем всё от хвоста до места куся.
                        var enemy = EnemyDetector.GetEnemyShake(gameBoard, newHead.X, newHead.Y);
                        return (enemy.tailLength + 1) * 10;
                    }
                    else
                        //4.2. Если нет - то текущая змейка погибает.
                        throw new Exception("Do it in filter!");
                //---me---
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
                    return -100;
                //---other---
                case BoardElement.None:
                    return 1;
                case BoardElement.Stone:
                    return gameBoard.EvilTicks > 0 ? 4 : (gameBoard.MyLength > 2 ? -100 : -1000);
                case BoardElement.Apple:
                    return 2;
                case BoardElement.Gold:
                    return 11;
                case BoardElement.FuryPill:
                    return 5;
                //
                default:
                    return long.MinValue;
            }
        }
    }
}
