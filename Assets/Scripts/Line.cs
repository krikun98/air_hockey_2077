using System.Drawing;
using UnityEngine;

namespace Mirror.AirHockey2077
{
    public class Line
    {
        public float A;
        public float B;
        public float C;

        public Line(float a, float b, float c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Line(float x1, float y1, float x2, float y2)
        {
            A = y1 - y2;
            B = x2 - x1;
            C = x1 * y2 - x2 * y1;
        }

        public Vector2 Intersect(Line line)
        {
            float d = A * line.B - line.A * B;
            if (d.Equals(0))
            {
                return Constants.undef;
            }

            float y = (line.A * C - A * line.C) / d;
            float x = (B * line.C - line.B * C) / d;
            return new Vector2(x, y);
        }
    }
}