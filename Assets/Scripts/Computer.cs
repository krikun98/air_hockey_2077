using UnityEngine;

namespace Mirror.AirHockey2077
{
    public abstract class Computer : NetworkBehaviour
    {
        protected readonly float speed = Constants.RacketSpeed;
        protected Vector2 undef = new Vector2(-100, -100);

        public Rigidbody2D rigidbody2d;
        protected float myY;
        protected Ball ball;
        protected bool predict;
        protected Vector2 currentPredictPoint;

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
        private void Update()
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
            currentPredictPoint = undef;
        }

        protected void MoveUp()
        {
            rigidbody2d.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }

        protected void MoveDown()
        {
            rigidbody2d.transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }
    }
}