using UnityEngine;

namespace Mirror.AirHockey2077
{
    public abstract class Computer : NetworkBehaviour
    {
        protected readonly float speed = Constants.RacketSpeed;

        public Rigidbody2D rigidbody2d;
        protected float myY;
        protected Ball ball;
        protected bool predict;
        protected Vector2 currentPredictPoint;
        protected float lastBallDir;

        public void UpdateBall(Ball newBall)
        {
            ball = newBall;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            // only simulate ball physics on server
            rigidbody2d.simulated = true;
        }

        [ServerCallback]
        private void FixedUpdate()
        {
            AI();
        }

        protected void UpdatePositions()
        {
            myY = rigidbody2d.position.y;
        }

        protected abstract void AI();

        public void ResetPrediction()
        {
            predict = false;
            currentPredictPoint = Constants.undef;
        }

		protected void Move(Vector3 movement) {
            rigidbody2d.transform.position += movement;
		}
    }
}