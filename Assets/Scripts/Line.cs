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

        public Vector2 Point(Line line)
        {
            float y = (-line.C + line.A / A * C) / (line.B - line.A / A * B);
            float x = (-C - B * y) / A;
            return new Vector2(x, y);
        }
    }
}