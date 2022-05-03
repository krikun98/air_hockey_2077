using System;
using UnityEngine;

namespace Mirror.AirHockey2077
{
    public class Utils
    {
        public static Tuple<bool, Line> NewDir(Line dir, Line wall)
        {
            var point = dir.Point(wall);
            if (IsInterestingPointX(point))
            {
                var newA = -dir.A;
                var newC = - newA * point.x - dir.B * point.y;
                return new Tuple<bool, Line>(true, new Line(newA, dir.B, newC));
            }
            return new Tuple<bool, Line>(false, null);
        }

        public static bool IsInterestingPointX(Vector2 point)
        {
            return point.x < Constants.RacketPosX;
        }
    }
}