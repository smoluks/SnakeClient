using SnakeBattle.Api;
using System;

namespace SnakeBattle.AI
{
    public static class PossiblyFilter
    {
        public static bool IsMovePossible(GameBoard gameBoard, SnakeAction actionS, SnakeAction lastMoveS, bool real, out Element newHead)
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
            if(Lists.IsHead(newHead.type))
                return false;
            if (Lists.IsBody(newHead.type) || Lists.IsTail(newHead.type))
            {
                var me = MeDetector.GetMe(gameBoard, newHead.X, newHead.Y);

                Console.WriteLine($"Me: {me.tailLength} {me.headLength} {me.rage}");
                if (me.headLength < 2)
                    return false;
            }

            //enemies
            if (Lists.IsEnemyHead(newHead.type) || (Lists.IsEnemyBody(newHead.type) && Helpers.IsNear(gameBoard, newHead.X, newHead.Y, Lists.enemyHeads)))
            {
                var enemyAgro = newHead.type == BoardElement.EnemyHeadEvil || Helpers.IsNear(gameBoard, newHead.X, newHead.Y, BoardElement.EnemyHeadEvil);

                //--правила для головы--
                //Если текущая змейка не в ярости, а вражеская в ярости - то текущая змейка умирает.
                if (gameBoard.EvilTicks == 0 && enemyAgro)
                    return false;
                //Если обе змейки в ярости или обе не в ярости - то от каждой отрезаем длину другой змейки. 
                //То есть, если столкнулись две змейки длиной в 5 и в 7 единиц, то змейка длиной в пять погибает, так как 5-7<2, а змейка длиной в 7 принимает длину 2 единицы.
                else if ((gameBoard.EvilTicks != 0) == enemyAgro)
                {
                    var enemy = EnemyDetector.GetEnemyShake(gameBoard, newHead.X, newHead.Y);
                    Console.WriteLine($"Enemy: {enemy.tailLength} {enemy.headLength} {enemy.rage}");

                    if (gameBoard.MyLength - (enemy.headLength + enemy.tailLength + 1) < 2)
                        return false;
                }
            }
            else if (Lists.IsEnemyBody(newHead.type) || Lists.IsEnemyTail(newHead.type))
            {
                if (gameBoard.EvilTicks == 0)
                    return false;
            }

            return true;
        }
    }
}
