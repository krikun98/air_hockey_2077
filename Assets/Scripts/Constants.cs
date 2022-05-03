namespace Mirror.AirHockey2077
{
    public static class Constants
    {
        //scene
        public static readonly float weight = 49;
        public static readonly float height = 32;
        public const float Top = 16;
        public const float Bottom = -16;

        //walls
        public static readonly float wallTopRightPos = 6.2f;
        public static readonly float wallBottomRightPos = -6.2f;
        public static readonly float wallWeight = 1;
        
        //computer racket
        public static readonly float racketPosX = 20;
        public static readonly float racketWeight = 2;
        public static readonly float racketSpeed = 30;
        public static readonly float racketLeft = racketPosX - racketWeight/2;
        public static readonly float racketRight = racketPosX + racketWeight/2;
        public static readonly float racketHeight = 4;
        public static readonly float racketMinY = wallWeight;
        public static readonly float racketMaxY = height - wallWeight / racketSpeed;
        public static readonly Line racketLine = new Line(1, 0, -racketPosX);
        
        //ball
        public static readonly float ballSpeed = 30;
        public static readonly float ballRadius = 0.5f;
        public static readonly Line topWallLine = new Line(0, 1, -Top);
        public static readonly Line bottomWallLine = new Line(0, 1, Bottom);
        
    }
}