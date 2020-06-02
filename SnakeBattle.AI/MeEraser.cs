using SnakeBattle.Api;
using System;

namespace SnakeBattle.AI
{
    public static class MeEraser
    {
        internal static bool CleanMe(GameBoard gameBoard, int x, int y)
        {
            int length1;
            int length2;
            bool isTail1;
            bool isTail2;

            var el = gameBoard.Board[x, y];
            switch (el)
            {
                //---heads---
                case BoardElement.HeadDown:
                case BoardElement.HeadUp:
                case BoardElement.HeadLeft:
                case BoardElement.HeadRight:
                case BoardElement.HeadEvil:
                    throw new SnakeException($"Inherit eat head at {x}:{y}", gameBoard);
                //bodies
                case BoardElement.BodyHorizontal:
                    isTail1 = IsTail(gameBoard, x - 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x + 1, y, x, y, out length2);

                    if (isTail1)
                        CleanShakeInternal(gameBoard, x - 1, y, x, y);
                    if (isTail2)
                        CleanShakeInternal(gameBoard, x + 1, y, x, y);
                    break;
                case BoardElement.BodyVertical:
                    isTail1 = IsTail(gameBoard, x, y + 1, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y - 1, x, y, out length2);

                    if (isTail1)
                        CleanShakeInternal(gameBoard, x, y + 1, x, y);
                    if (isTail2)
                        CleanShakeInternal(gameBoard, x, y - 1, x, y);
                    break;
                case BoardElement.BodyLeftDown:
                    isTail1 = IsTail(gameBoard, x - 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y + 1, x, y, out length2);

                    if (isTail1)
                        CleanShakeInternal(gameBoard, x - 1, y, x, y);
                    if (isTail2)
                        CleanShakeInternal(gameBoard, x, y + 1, x, y);
                    break;
                case BoardElement.BodyLeftUp:
                    isTail1 = IsTail(gameBoard, x - 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y - 1, x, y, out length2);

                    if (isTail1)
                        CleanShakeInternal(gameBoard, x - 1, y, x, y);
                    if (isTail2)
                        CleanShakeInternal(gameBoard, x, y - 1, x, y);
                    break;
                case BoardElement.BodyRightDown:
                    isTail1 = IsTail(gameBoard, x + 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y + 1, x, y, out length2);

                    if (isTail1)
                        CleanShakeInternal(gameBoard, x + 1, y, x, y);
                    if (isTail2)
                        CleanShakeInternal(gameBoard, x, y + 1, x, y);
                    break;
                case BoardElement.BodyRightUp:
                    isTail1 = IsTail(gameBoard, x + 1, y, x, y, out length1);
                    isTail2 = IsTail(gameBoard, x, y - 1, x, y, out length2);

                    if (isTail1)
                        CleanShakeInternal(gameBoard, x + 1, y, x, y);
                    if (isTail2)
                        CleanShakeInternal(gameBoard, x, y - 1, x, y);
                    break;
                default:
                    return false;
            }

            //make new tail
            var n = Helpers.FindMeBody(gameBoard, x, y);
            if (n.X == -1)
            {
                throw new SnakeException($"Not found body near {x}:{y}", gameBoard);
            }

            gameBoard.Board[x, y] = BoardElement.None;

            var n2 = Helpers.FindMeBody(gameBoard, n.X, n.Y, Lists.bodies);
            if (n2.X == -1)
            {
                throw new SnakeException($"Not found body near new tail at {n.X}:{n.Y}", gameBoard);
            }

            if (n2.X - n.X == 1)
            {
                //шея справа
                gameBoard.Board[n.X, n.Y] = BoardElement.TailEndLeft;
                gameBoard.TailType = BoardElement.TailEndLeft;
                gameBoard.Tail.X = n.X;
                gameBoard.Tail.Y = n.Y;
            }
            else if (n2.X - n.X == -1)
            {
                //шея слева
                gameBoard.Board[n.X, n.Y] = BoardElement.TailEndRight;
                gameBoard.TailType = BoardElement.TailEndRight;
                gameBoard.Tail.X = n.X;
                gameBoard.Tail.Y = n.Y;
            }
            else if (n2.Y - n.Y == 1)
            {
                //шея снизу
                gameBoard.Board[n.X, n.Y] = BoardElement.TailEndUp;
                gameBoard.TailType = BoardElement.TailEndUp;
                gameBoard.Tail.X = n.X;
                gameBoard.Tail.Y = n.Y;
            }
            else if (n2.Y - n.Y == -1)
            {
                //шея сверху
                gameBoard.Board[n.X, n.Y] = BoardElement.TailEndDown;
                gameBoard.TailType = BoardElement.TailEndDown;
                gameBoard.Tail.X = n.X;
                gameBoard.Tail.Y = n.Y;
            }

            return true;
        }

        private static void CleanShakeInternal(GameBoard gameBoard, int x, int y, int prevx, int prevy)
        {
            Element e1;
            Element e2;

            var el = gameBoard.Board[x, y];
            switch (el)
            {
                //---heads---
                case BoardElement.HeadDown:
                case BoardElement.HeadUp:
                case BoardElement.HeadLeft:
                case BoardElement.HeadRight:
                case BoardElement.HeadEvil:
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    return;
                //---tails----
                case BoardElement.TailEndDown:
                case BoardElement.TailEndUp:
                case BoardElement.TailEndLeft:
                case BoardElement.TailEndRight:
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    return;
                //bodies
                case BoardElement.BodyHorizontal:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x + 1, y);
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    break;
                case BoardElement.BodyVertical:
                    e1 = new Element(x, y + 1);
                    e2 = new Element(x, y - 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    break;
                case BoardElement.BodyLeftDown:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y + 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    break;
                case BoardElement.BodyLeftUp:
                    e1 = new Element(x - 1, y);
                    e2 = new Element(x, y - 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    break;
                case BoardElement.BodyRightDown:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y + 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    break;
                case BoardElement.BodyRightUp:
                    e1 = new Element(x + 1, y);
                    e2 = new Element(x, y - 1);
                    gameBoard.Board[x, y] = BoardElement.None;
                    gameBoard.MyLength--;
                    break;
                default:
                    return;
                    //Console.Clear();
                    //Graphical.WriteField(gameBoard);
                    //throw new Exception("Not a me");
            }

            if (e1.X != prevx || e1.Y != prevy)
            {
                CleanShakeInternal(gameBoard, e1.X, e1.Y, x, y);
            }
            else
            {
                CleanShakeInternal(gameBoard, e2.X, e2.Y, x, y);
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
                case BoardElement.HeadDown:
                case BoardElement.HeadUp:
                case BoardElement.HeadLeft:
                case BoardElement.HeadRight:
                case BoardElement.HeadEvil:
                    length = 1;
                    return false;
                //---tails----
                case BoardElement.TailEndDown:
                case BoardElement.TailEndUp:
                case BoardElement.TailEndLeft:
                case BoardElement.TailEndRight:
                    length = 1;
                    return true;
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
                    length = 0;
                    return true;
                    //Console.Clear();
                    //Graphical.WriteField(gameBoard);
                    //throw new Exception("Not a me");
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
