using System;
using UnityEngine;
using Random = System.Random;

namespace Mirror.AirHockey2077
{
    public class SmartComputer : Computer
    {
        private const float AIReaction = 0.05f;
        private float _waitingTime;

        public float aiError = 3;
        private int _ballDir = 0;

        protected override void AI()
        {
            ball.UpdatePosition();
            if (ball.Dx() > 0 && _ballDir <= 0)
            {
                _ballDir = 1;
                ResetPrediction();
            }

            if (ball.Dx() < 0 && _ballDir >= 0)
            {
                _ballDir = -1;
                ResetPrediction();
            }

            UpdatePositions();
            Predict();
        }

        private void Predict()
        {
            // if (_waitingTime < AIReaction && !predict)
            // {
            //     _waitingTime += Time.deltaTime;
            //     return;
            // }

            if (!predict)
            {
                currentPredictPoint = BallIntercept();
            }

            if (currentPredictPoint != _undef)
            {
                GoToPos(currentPredictPoint);
            }
        }

        private void GoToPos(Vector2 point)
        {
            if (point.y > myY && point.y < Constants.wallTopRightPos)
            {
                MoveUp();
            }

            if (point.y < myY && point.y > Constants.wallBottomRightPos)
            {
                MoveDown();
            }
        }

        private Vector2 BallIntercept()
        {
            float dx = ball.Dx();
            float dy = ball.Dy();

            if (dx <= 0 || predict)
            {
                return _undef;
            }

            Line lastDir = new Line(0, 0 , 0);
            Line currentDirLine = ball.Dir();
            while (!predict)
            {
                if (dx > 0 && dy > 0)
                {
                    var newDirPair = Utils.NewDir(Constants.topWallLine, currentDirLine);
                    if (newDirPair.Item1)
                    {
                        currentDirLine = newDirPair.Item2;
                        dy = -dy;
                    }
                    else
                    {
                        lastDir = currentDirLine;
                        predict = true;
                        _waitingTime = 0;
                    }
                }

                if (dx > 0 && dy < 0)
                {
                    var newDirPair = Utils.NewDir(Constants.bottomWallLine, currentDirLine);
                    if (newDirPair.Item1)
                    {
                        currentDirLine = newDirPair.Item2;
                        dy = -dy;
                    }
                    else
                    {
                        lastDir = currentDirLine;
                        predict = true;
                        _waitingTime = 0;
                    }
                }
            }

            return lastDir.Point(Constants.racketLine);
        }

        private float Random(float min, float max)
        {
            return (min + ((new Random()).Next() * (max - min)));
        }
    }
}