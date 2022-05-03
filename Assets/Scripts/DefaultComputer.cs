using UnityEngine;
using System;

namespace Mirror.AirHockey2077
{
    public class DefaultComputer : Computer
    {
        protected override void AI()
        {
            ball.UpdatePosition();
            UpdatePositions();
            if (myY < ball.Y() && myY < Constants.WallTopRightPos)
            {
                if (Math.Abs(ball.Y() - myY) < Time.fixedDeltaTime * speed)
                {
                    Move(new Vector3(0, Math.Abs(ball.Y() - myY), 0));
                }
                else
                {
                    Move(new Vector3(0, speed * Time.fixedDeltaTime, 0));
                }
            }

            if (myY > ball.Y() && myY > Constants.WallBottomRightPos)
            {
                if (Math.Abs(ball.Y() - myY) < Time.fixedDeltaTime * speed)
                {
                    Move(new Vector3(0, -Math.Abs(ball.Y() - myY), 0));
                }
                else
                {
                    Move(new Vector3(0, -speed * Time.fixedDeltaTime, 0));
                }
            }
        }
    }
}