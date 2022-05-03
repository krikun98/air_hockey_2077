namespace Mirror.AirHockey2077
{
    public static class Constants
    {
        //scene
        public const float Top = 6;
        public const float Bottom = -6;

        //walls
        public const float WallTopRightPos = 2.5f;
        public const float WallBottomRightPos = -2.5f;

        //computer racket
        public const float RacketPosX = 8;
        public const float RacketSpeed = 5;

        // Constant Lines
        public static readonly Line topWallLine = new Line(0, 1, -Top);
        public static readonly Line bottomWallLine = new Line(0, 1, Bottom);
        public static readonly Line racketLine = new Line(1, 0, -RacketPosX);
    }
}