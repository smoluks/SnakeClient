using SnakeBattle.Api;
using System;

namespace SnakeBattle.AI
{
    internal static class MeDetector
    {
        internal static (int headLength, int tailLength, bool isEvilNear) GetMe(GameBoard gameBoard, int x, int y)
        {
            int length1;
            bool head1;
            bool evil1;
            int length2;
            bool head2;
            bool evil2;

            var el = gameBoard.Board[x, y];
            switch (el)
            {
                //---heads---
                case BoardElement.HeadDown:
                    GetMeShakeInternal(gameBoard, x, y - 1, x, y, out length1, out head1, out evil1);
                    return (0, length1, evil1);
                case BoardElement.HeadUp:
                    GetMeShakeInternal(gameBoard, x, y + 1, x, y, out length1, out head1, out evil1);
                    return (0, length1, evil1);
                case BoardElement.HeadLeft:
                    GetMeShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    return (0, length1, evil1);
                case BoardElement.HeadRight:
                    GetMeShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    return (0, length1, evil1);
                case BoardElement.HeadEvil:
                    throw new SnakeException("Non-masked evil head", gameBoard);
                //---tails----
                case BoardElement.TailEndDown:
                    GetMeShakeInternal(gameBoard, x, y - 1, x, y, out length1, out head1, out evil1);
                    return (length1,0, evil1);
                case BoardElement.TailEndUp:
                    GetMeShakeInternal(gameBoard, x, y + 1, x, y, out length1, out head1, out evil1);
                    return (length1, 0, evil1);
                case BoardElement.TailEndLeft:
                    GetMeShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    return (length1, 0, evil1);
                case BoardElement.TailEndRight:
                    GetMeShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    return (length1, 0, evil1);
                //bodies
                case BoardElement.BodyHorizontal:
                    GetMeShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    GetMeShakeInternal(gameBoard, x + 1, y, x, y, out length2, out head2, out evil2);
                    if(head1)
                        return (length1, length2, evil1 || evil2);
                    else
                        return (length2, length1, evil1 || evil2);
                case BoardElement.BodyVertical:
                    GetMeShakeInternal(gameBoard, x, y + 1, x, y, out length1, out head1, out evil1);
                    GetMeShakeInternal(gameBoard, x, y - 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1 || evil2);
                    else
                        return (length2, length1, evil1 || evil2);
                case BoardElement.BodyLeftDown:
                    GetMeShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    GetMeShakeInternal(gameBoard, x, y + 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1 || evil2);
                    else
                        return (length2, length1, evil1 || evil2);
                case BoardElement.BodyLeftUp:
                    GetMeShakeInternal(gameBoard, x - 1, y, x, y, out length1, out head1, out evil1);
                    GetMeShakeInternal(gameBoard, x, y - 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1 || evil2);
                    else
                        return (length2, length1, evil1 || evil2);
                case BoardElement.BodyRightDown:
                    GetMeShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    GetMeShakeInternal(gameBoard, x, y + 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1 || evil2);
                    else
                        return (length2, length1, evil1 || evil2);
                case BoardElement.BodyRightUp:
                    GetMeShakeInternal(gameBoard, x + 1, y, x, y, out length1, out head1, out evil1);
                    GetMeShakeInternal(gameBoard, x, y - 1, x, y, out length2, out head2, out evil2);
                    if (head1)
                        return (length1, length2, evil1 || evil2);
                    else
                        return (length2, length1, evil1 || evil2);
                default:
                    throw new SnakeException($"Not a me at {x}:{y}", gameBoard);
            }
        }

        private static void GetMeShakeInternal(GameBoard gameBoard, int x, int y, int prevx, int prevy, out int length, out bool head, out bool isEvilNear)
        {
            Element e1;
            Element e2;
            var el = gameBoard.Board[x, y];
            isEvilNear = Helpers.IsNear(gameBoard, x, y, BoardElement.EnemyHeadEvil, 5);

            switch (el)
            {
                //---heads---
                case BoardElement.HeadDown:
                case BoardElement.HeadUp:
                case BoardElement.HeadLeft:
                case BoardElement.HeadRight:
                    length = 1;
                    head = true;
                    return;
                case BoardElement.HeadEvil:
                    length = 1;
                    head = true;
                    return;
                //---tails----
                case BoardElement.TailEndDown:
                case BoardElement.TailEndUp:
                case BoardElement.TailEndLeft:
                case BoardElement.TailEndRight:
                    length = 1;
                    head = false;
                    return;
                //bodies
                case BoardElement.BodyHorizontal:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x + 1, y);
                    break;
                case BoardElement.BodyVertical:
                    e1 = new Element(x, y + 1);
                    e2 = new Element(x, y - 1);
                    break;
                case BoardElement.BodyLeftDown:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y + 1);
                    break;
                case BoardElement.BodyLeftUp:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y - 1);
                    break;
                case BoardElement.BodyRightDown:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y + 1);
                    break;
                case BoardElement.BodyRightUp:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y - 1);
                    break;
                default:
                    throw new SnakeException($"Not a me: {el}");
                    //length = 0;
                    // head = false;
                   // evil = false;
                    //return;
            }

            if(e1.X != prevx || e1.Y != prevy)
            {
                GetMeShakeInternal(gameBoard, e1.X, e1.Y, x, y, out var length1, out var head1, out var evil1);

                length = length1 + 1;
                head = head1;
                isEvilNear |= evil1;
                return;
            }
            else
            {
                GetMeShakeInternal(gameBoard, e2.X, e2.Y, x, y, out var length2, out var head2, out var evil2);

                length = length2 + 1;
                head = head2;
                isEvilNear |= evil2;
                return;
            }
        }
    }
}
