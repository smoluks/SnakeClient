namespace SnakeBattle.Api
{
    public static class Lists
    {
        public static readonly BoardElement[] evilHeads = new BoardElement[] {
            BoardElement.EnemyHeadDown,
            BoardElement.EnemyHeadLeft,
            BoardElement.EnemyHeadRight,
            BoardElement.EnemyHeadUp,
            BoardElement.EnemyHeadEvil
        };

        public static bool IsEnemyHead(BoardElement el)
        {
            return (el == BoardElement.EnemyHeadDown) ||
                (el == BoardElement.EnemyHeadLeft) ||
                (el == BoardElement.EnemyHeadRight) ||
                (el == BoardElement.EnemyHeadUp) ||
                (el == BoardElement.EnemyHeadEvil);
        }

        public static readonly BoardElement[] evilBodies = new BoardElement[] {
            BoardElement.EnemyBodyHorizontal,
            BoardElement.EnemyBodyVertical,
            BoardElement.EnemyBodyLeftDown,
            BoardElement.EnemyBodyLeftUp,
            BoardElement.EnemyBodyRightDown,
            BoardElement.EnemyBodyRightUp,
        };

        public static bool IsEnemyBody(BoardElement el)
        {
            return (el == BoardElement.EnemyBodyHorizontal) ||
                (el == BoardElement.EnemyBodyVertical) ||
                (el == BoardElement.EnemyBodyLeftDown) ||
                (el == BoardElement.EnemyBodyLeftUp) ||
                (el == BoardElement.EnemyBodyRightDown) ||
                (el == BoardElement.EnemyBodyRightUp) ||
                (el == BoardElement.EnemyBodyAbstract);
        }

        public static readonly BoardElement[] evilTails = new BoardElement[] {
            BoardElement.EnemyTailEndDown,
            BoardElement.EnemyTailEndLeft,
            BoardElement.EnemyTailEndUp,
            BoardElement.EnemyTailEndRight,
            BoardElement.EnemyTailInactive,
        };

        public static bool IsEnemyTail(BoardElement el)
        {
            return (el == BoardElement.EnemyTailEndDown) ||
                (el == BoardElement.EnemyTailEndLeft) ||
                (el == BoardElement.EnemyTailEndUp) ||
                (el == BoardElement.EnemyTailEndRight) ||
                (el == BoardElement.EnemyTailInactive);
        }

        static readonly BoardElement[] heads = new BoardElement[] {
            BoardElement.HeadDown,
            BoardElement.HeadLeft,
            BoardElement.HeadRight,
            BoardElement.HeadUp,
            BoardElement.HeadEvil
        };

        public static bool IsHead(BoardElement el)
        {
            return (el == BoardElement.HeadDown) ||
                (el == BoardElement.HeadLeft) ||
                (el == BoardElement.HeadRight) ||
                (el == BoardElement.HeadUp) ||
                (el == BoardElement.HeadEvil);
        }

        static readonly BoardElement[] bodies = new BoardElement[] {
            BoardElement.BodyHorizontal,
            BoardElement.BodyVertical,
            BoardElement.BodyLeftDown,
            BoardElement.BodyLeftUp,
            BoardElement.BodyRightDown,
            BoardElement.BodyRightUp,
        };

        public static bool IsBody(BoardElement el)
        {
            return (el == BoardElement.BodyHorizontal) ||
                (el == BoardElement.BodyVertical) ||
                (el == BoardElement.BodyLeftDown) ||
                (el == BoardElement.BodyLeftUp) ||
                (el == BoardElement.BodyRightDown) ||
                (el == BoardElement.BodyRightUp);
        }

        static readonly BoardElement[] tails = new BoardElement[] {
            BoardElement.TailEndDown,
            BoardElement.TailEndLeft,
            BoardElement.TailEndUp,
            BoardElement.TailEndRight,
            BoardElement.TailInactive,
        };

        public static bool IsTail(BoardElement el)
        {
            return (el == BoardElement.TailEndDown) ||
                (el == BoardElement.TailEndLeft) ||
                (el == BoardElement.TailEndUp) ||
                (el == BoardElement.TailEndRight) ||
                (el == BoardElement.TailInactive);
        }

        public static bool IsNyamka(BoardElement el)
        {
            return (el == BoardElement.Apple) ||
                (el == BoardElement.Gold) ||
                (el == BoardElement.FuryPill);
        }
    }
}
