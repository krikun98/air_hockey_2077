using UnityEngine;

namespace Mirror.AirHockey2077
{
    public class DefaultComputer : Computer
    {
        protected override void AI()
        {
            ball.UpdatePosition();
            UpdatePositions();
            if (myY < ball.Y() && myY < Constants.wallTopRightPos)
            {
                MoveUp();
            }

            if (myY > ball.Y() && myY > Constants.wallBottomRightPos)
            {
                MoveDown();
            }
        }
    }
}