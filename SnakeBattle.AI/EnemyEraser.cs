using SnakeBattle.Api;
using System;

namespace SnakeBattle.AI
{
    public static class EnemyEraser
    {
        internal static void CleanEnemyShake(GameBoard gameBoard, int x, int y)
        {
            int length1;
            int length2;
            bool isTail1;
            bool isTail2;

            switch (gameBoard.Board[x, y])
            {
                //---heads---
                case BoardElement.EnemyHeadDown:
                    CleanEnemyShakeInternal(gameBoard, x, y - 1, x, y);
                    break;
                case BoardElement.EnemyHeadUp:
                    CleanEnemyShakeInternal(gameBoard, x, y + 1, x, y);
                    break;
                case BoardElement.EnemyHeadLeft:
                    CleanEnemyShakeInternal(gameBoard, x + 1, y, x, y);
                    break;
                case BoardElement.EnemyHeadRight:
                    CleanEnemyShakeInternal(gameBoard, x - 1, y, x, y);
                    break;
                case BoardElement.EnemyHeadEvil:
                    var body = Helpers.FindContinueEvilBody(gameBoard, x, y);
                    if (body.X == -1)
                        break;

                    CleanEnemyShakeInternal(gameBoard, body.X, body.Y, x, y);
                    break;
                //---tails----
                case BoardElement.EnemyTailEndDown:
                case BoardElement.EnemyTailEndUp:
                case BoardElement.EnemyTailEndLeft:
                case BoardElement.EnemyTailEndRight:
                    break;
                //bodies
                case BoardElement.EnemyBodyHorizontal:
                    isTail1 = IsTail(gameBoard, x - 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x + 1, y, x, y, out length2);

                    if (isTail1 || length1 < 2)
                        CleanEnemyShakeInternal(gameBoard, x - 1, y, x, y);
                    if (isTail2 || length2 < 2)
                        CleanEnemyShakeInternal(gameBoard, x + 1, y, x, y);
                    break;
                case BoardElement.EnemyBodyVertical:
                    isTail1 = IsTail(gameBoard, x, y + 1, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y - 1, x, y, out length2);

                    if (isTail1 || length1 < 2)
                        CleanEnemyShakeInternal(gameBoard, x, y + 1, x, y);
                    if (isTail2 || length2 < 2)
                        CleanEnemyShakeInternal(gameBoard, x, y - 1, x, y);
                    break;
                case BoardElement.EnemyBodyLeftDown:
                    isTail1 = IsTail(gameBoard, x - 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y + 1, x, y, out length2);

                    if (isTail1 || length1 < 2)
                        CleanEnemyShakeInternal(gameBoard, x - 1, y, x, y);
                    if (isTail2 || length2 < 2)
                        CleanEnemyShakeInternal(gameBoard, x, y + 1, x, y);
                    break;
                case BoardElement.EnemyBodyLeftUp:
                    isTail1 = IsTail(gameBoard, x - 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y - 1, x, y, out length2);

                    if (isTail1 || length1 < 2)
                        CleanEnemyShakeInternal(gameBoard, x - 1, y, x, y);
                    if (isTail2 || length2 < 2)
                        CleanEnemyShakeInternal(gameBoard, x, y - 1, x, y);
                    break;
                case BoardElement.EnemyBodyRightDown:
                    isTail1 = IsTail(gameBoard, x + 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y + 1, x, y, out length2);

                    if (isTail1 || length1 < 2)
                        CleanEnemyShakeInternal(gameBoard, x + 1, y, x, y);
                    if (isTail2 || length2 < 2)
                        CleanEnemyShakeInternal(gameBoard, x, y + 1, x, y);
                    break;
                case BoardElement.EnemyBodyRightUp:
                    isTail1 = IsTail(gameBoard, x + 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y - 1, x, y, out length2);

                    if (isTail1 || length1 < 2)
                        CleanEnemyShakeInternal(gameBoard, x + 1, y, x, y);
                    if (isTail2 || length2 < 2)
                        CleanEnemyShakeInternal(gameBoard, x, y - 1, x, y);
                    break;
                default:
                    return;
            }

            gameBoard.Board[x, y] = BoardElement.None;

            //make new tail
            var n = Helpers.FindEvilBody(gameBoard, x, y);
            if (n.X == -1)
            {
                return;
            }

            //gameBoard.Board[n.X, n.Y] = BoardElement.None;
            var n2 = Helpers.FindEvilBody(gameBoard, n.X, n.Y);
            if (n2.X == -1)
                return;

            if (n2.X - n.X == 1)
            {
                //шея справа
                gameBoard.Board[n.X, n.Y] = BoardElement.EnemyTailEndLeft;
            }
            else if (n2.X - n.X == -1)
            {
                //шея слева
                gameBoard.Board[n.X, n.Y] = BoardElement.EnemyTailEndRight;
            }
            else if (n2.Y - n.Y == 1)
            {
                //шея снизу
                gameBoard.Board[n.X, n.Y] = BoardElement.EnemyTailEndUp;
            }
            else if (n2.Y - n.Y == -1)
            {
                //шея сверху
                gameBoard.Board[n.X, n.Y] = BoardElement.EnemyTailEndDown;
            }

        }

        private static void CleanEnemyShakeInternal(GameBoard gameBoard, int x, int y, int prevx, int prevy)
        {
            Element e1;
            Element e2;

            var el = gameBoard.Board[x, y];
            switch (el)
            {
                //---heads---
                case BoardElement.EnemyHeadDown:
                case BoardElement.EnemyHeadUp:
                case BoardElement.EnemyHeadLeft:
                case BoardElement.EnemyHeadRight:
                case BoardElement.EnemyHeadEvil:
                    gameBoard.Board[x, y] = BoardElement.None;
                    return;
                //---tails----
                case BoardElement.EnemyTailEndDown:
                case BoardElement.EnemyTailEndUp:
                case BoardElement.EnemyTailEndLeft:
                case BoardElement.EnemyTailEndRight:
                    gameBoard.Board[x, y] = BoardElement.None;
                    return;
                //bodies
                case BoardElement.EnemyBodyHorizontal:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x + 1, y);
                    gameBoard.Board[x, y] = BoardElement.None;
                    break;
                case BoardElement.EnemyBodyVertical:
                    e1 = new Element(x, y + 1);
                    e2 = new Element(x, y - 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    break;
                case BoardElement.EnemyBodyLeftDown:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y + 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    break;
                case BoardElement.EnemyBodyLeftUp:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y - 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    break;
                case BoardElement.EnemyBodyRightDown:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y + 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    break;
                case BoardElement.EnemyBodyRightUp:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y - 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    break;
                default:
                    //уже слопали
                    return;
            }

            if (e1.X != prevx || e1.Y != prevy)
            {
                CleanEnemyShakeInternal(gameBoard, e1.X, e1.Y, x, y);
            }
            else
            {
                CleanEnemyShakeInternal(gameBoard, e2.X, e2.Y, x, y);
            }
        }

        private static bool IsTail(GameBoard gameBoard, int x, int y, int prevx, int prevy, out int length)
        {
            Element e1;
            Element e2;

            var el = gameBoard.Board[x, y];
            switch (el)
            {
                //---heads---
                case BoardElement.EnemyHeadDown:
                case BoardElement.EnemyHeadUp:
                case BoardElement.EnemyHeadLeft:
                case BoardElement.EnemyHeadRight:
                case BoardElement.EnemyHeadEvil:
                    length = 1;
                    return false;
                //---tails----
                case BoardElement.EnemyTailEndDown:
                case BoardElement.EnemyTailEndUp:
                case BoardElement.EnemyTailEndLeft:
                case BoardElement.EnemyTailEndRight:
                    length = 1;
                    return true;
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
                    length = 1;
                    return true;
                    //Console.Clear();
                    //Graphical.WriteField(gameBoard);
                    //throw new Exception("Not a enemy");
            }


            if (e1.X != prevx || e1.Y != prevy)
            {
                var r = IsTail(gameBoard, e1.X, e1.Y, x, y, out length);
                length++;
                return r;
            }
            else
            {
                var r = IsTail(gameBoard, e2.X, e2.Y, x, y, out length);
                length++;
                return r;
            }

        }
    }
}
