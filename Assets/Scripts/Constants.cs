using UnityEngine;

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
        public const float RacketSpeed = 7;

        // Constant Lines
        public static readonly Line racketLine = new Line(1, 0, -RacketPosX);
        
        public static readonly Vector2 undef = new Vector2(-100, -100);

        public const float BallSpeed = 30;
        public const float ObstacleSpeed = 10;
        public const float RotationSpeed = 1000;
        public const int MaxObstacleCount = 3;

        public const float IgnoreThreshold = 0.5f;
    }
}