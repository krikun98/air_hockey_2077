using System;
using UnityEngine;
using Random = System.Random;

namespace Mirror.AirHockey2077
{
    public class SmartComputer : Computer
    {
        public int count;

        protected override void AI()
        {
            ball.UpdatePosition();
            if (ball.Dx() > 0 && lastBallDir <= 0)
            {
                ResetPrediction();
            }

            lastBallDir = ball.Dx();

            UpdatePositions();
            if (ball.Dx() < 0)
            {
                return;
            }

            if (!predict)
            {
                Predict();
            }
            else
            {
                GoToPos(currentPredictPoint);
            }
        }

        private void Predict()
        {
            count++;
            Debug.Log("Predict: " + count);
            currentPredictPoint = BallIntercept();

            if (currentPredictPoint != Constants.undef)
            {
                predict = true;
                GoToPos(currentPredictPoint);
            }
        }

        private void GoToPos(Vector2 point)
        {
            if (point.y > myY && point.y < Constants.Top)
            {
                if (Math.Abs(point.y - myY) < Time.fixedDeltaTime * speed)
                {
                    Move(new Vector3(0, Math.Abs(point.y - myY), 0));
                }
                else
                {
                    Move(new Vector3(0, speed * Time.fixedDeltaTime, 0));
                }
            }

            if (point.y < myY && point.y > Constants.Bottom)
            {
                if (Math.Abs(point.y - myY) < Time.fixedDeltaTime * speed)
                {
                    Move(new Vector3(0, -Math.Abs(point.y - myY), 0));
                }
                else
                {
                    Move(new Vector3(0, -speed * Time.fixedDeltaTime, 0));
                }
            }
        }

        private Vector2 BallIntercept()
        {
            var dx = ball.Dx();
            var dy = ball.Dy();

            // Line lastDir = new Line(0, 0, 0);
            Line currentDirLine = ball.Dir();

            if (dx > 0 && dy > 0)
            {
                var pt = currentDirLine.Intersect(Constants.racketLine);

                var newY = (Constants.Top + pt.y);
                var blockCount = (int) (newY / (2 * Constants.Top));
                var mod = newY % (2 * Constants.Top);
                if (blockCount % 2 == 1)
                {
                    return new Vector2(Constants.RacketPosX, Constants.Top - mod);
                }

                if (blockCount % 2 == 0)
                {
                    return new Vector2(Constants.RacketPosX, -Constants.Top + mod);
                }
            }
            else if (dx > 0 && dy < 0)
            {
                var pt = currentDirLine.Intersect(Constants.racketLine);

                var newY = (Constants.Top - pt.y);
                var blockCount = (int) (newY / (2 * Constants.Top));
                var mod = newY % (2 * Constants.Top);
                if (blockCount % 2 == 0)
                {
                    return new Vector2(Constants.RacketPosX, Constants.Top - mod);
                }

                if (blockCount % 2 == 1)
                {
                    return new Vector2(Constants.RacketPosX, -Constants.Top + mod);
                }
            }
            return Constants.undef;
        }
    }
}