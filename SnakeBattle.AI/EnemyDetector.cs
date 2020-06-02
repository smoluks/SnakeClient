using SnakeBattle.Api;
using System;

namespace SnakeBattle.AI
{
    internal static class EnemyDetector
    {
        
        internal static (int headLength, int tailLength, bool rage) GetEnemyShake(GameBoard gameBoard, int x, int y)
        {
            int length1;
            bool head1;
            bool evil1;
            int length2;
            bool head2;
            bool evil2;

            switch (gameBoard.Board[x, y])
            {
                //---heads---
                case BoardElement.EnemyHeadDown:
                    GetEnemyShakeInternal(gameBoard, x, y - 1, x, y, out length1, out head1, out evil1);
                    return (0, length1, false);
                case BoardElement.EnemyHeadUp:
                    GetEnemyShakeInternal(gameBoard, x, y + 1, x, y, out length1, out head1, out evil1);
                    return (0, length1, false);
                case BoardElement.EnemyHeadLeft:
                    GetEnemyShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    return (0, length1, false);
                case BoardElement.EnemyHeadRight:
                    GetEnemyShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    return (0, length1, false);
                case BoardElement.EnemyHeadEvil:
                    var body = Helpers.FindContinueEvilBody(gameBoard, x, y);
                    GetEnemyShakeInternal(gameBoard, body.X, body.Y, x, y, out length1, out head1, out evil1);
                    return (0, length1, true);
                //---tails----
                case BoardElement.EnemyTailEndDown:
                    GetEnemyShakeInternal(gameBoard, x, y - 1, x, y, out length1, out head1, out evil1);
                    return (length1,0, evil1);
                case BoardElement.EnemyTailEndUp:
                    GetEnemyShakeInternal(gameBoard, x, y + 1, x, y, out length1, out head1, out evil1);
                    return (length1, 0, evil1);
                case BoardElement.EnemyTailEndLeft:
                    GetEnemyShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    return (length1, 0, evil1);
                case BoardElement.EnemyTailEndRight:
                    GetEnemyShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    return (length1, 0, evil1);
                //bodies
                case BoardElement.EnemyBodyHorizontal:
                    GetEnemyShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    GetEnemyShakeInternal(gameBoard, x + 1, y, x, y, out length2, out head2, out evil2);
                    if(head1)
                        return (length1, length2, evil1);
                    else
                        return (length2, length1, evil2);
                case BoardElement.EnemyBodyVertical:
                    GetEnemyShakeInternal(gameBoard, x, y + 1, x, y, out length1, out head1, out evil1);
                    GetEnemyShakeInternal(gameBoard, x, y - 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1);
                    else
                        return (length2, length1, evil2);
                case BoardElement.EnemyBodyLeftDown:
                    GetEnemyShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    GetEnemyShakeInternal(gameBoard, x, y + 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1);
                    else
                        return (length2, length1, evil2);
                case BoardElement.EnemyBodyLeftUp:
                    GetEnemyShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    GetEnemyShakeInternal(gameBoard, x, y - 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1);
                    else
                        return (length2, length1, evil2);
                case BoardElement.EnemyBodyRightDown:
                    GetEnemyShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    GetEnemyShakeInternal(gameBoard, x, y + 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1);
                    else
                        return (length2, length1, evil2);
                case BoardElement.EnemyBodyRightUp:
                    GetEnemyShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    GetEnemyShakeInternal(gameBoard, x, y - 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1);
                    else
                        return (length2, length1, evil2);
                default:
                    //return (0, 0, false);
                    throw new SnakeException($"Not a enemy at {x}:{y}", gameBoard);
            }
        }

        private static void GetEnemyShakeInternal(GameBoard gameBoard, int x, int y, int prevx, int prevy, out int length, out bool head, out bool evil)
        {
            Element e1;
            Element e2;
            if (x == -1)
            {
                //мы уже это слопали
                length = 0;
                head = false;
                evil = false;
                return;
            }

            var el = gameBoard.Board[x, y];
            switch (el)
            {
                //---heads---
                case BoardElement.EnemyHeadDown:
                case BoardElement.EnemyHeadUp:
                case BoardElement.EnemyHeadLeft:
                case BoardElement.EnemyHeadRight:
                case BoardElement.EnemyHeadDead:
                    length = 1;
                    head = true;
                    evil = false;
                    return;
                case BoardElement.EnemyHeadEvil:
                    length = 1;
                    head = true;
                    evil = true;
                    return;
                //---tails----
                case BoardElement.EnemyTailEndDown:
                case BoardElement.EnemyTailEndUp:
                case BoardElement.EnemyTailEndLeft:
                case BoardElement.EnemyTailEndRight:
                    length = 1;
                    head = false;
                    evil = false;
                    return;
                //bodies
                case BoardElement.EnemyBodyHorizontal:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x + 1, y);
                    break;
                case BoardElement.EnemyBodyVertical:
                    e1 = new Element(x, y + 1);
                    e2 = new Element(x, y - 1);
                    break;
                case BoardElement.EnemyBodyLeftDown:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y + 1);
                    break;
                case BoardElement.EnemyBodyLeftUp:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y - 1);
                    break;
                case BoardElement.EnemyBodyRightDown:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y + 1);
                    break;
                case BoardElement.EnemyBodyRightUp:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y - 1);
                    break;
                default:
                    //мы уже это слопали
                    length = 0;
                    head = false;
                    evil = false;
                    return;
            }

            if (e1.X != prevx || e1.Y != prevy)
            {
                GetEnemyShakeInternal(gameBoard, e1.X, e1.Y, x, y, out var length1, out var head1, out var evil1);

                length = length1 + 1;
                head = head1;
                evil = evil1;
                return;
            }
            else
            {
                GetEnemyShakeInternal(gameBoard, e2.X, e2.Y, x, y, out var length2, out var head2, out var evil2);

                length = length2 + 1;
                head = head2;
                evil = evil2;
                return;
            }

        }
    }
}
