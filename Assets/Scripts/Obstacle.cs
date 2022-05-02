using UnityEngine;

namespace Mirror.AirHockey2077
{
    public class Obstacle : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;

        public override void OnStartServer()
        {
            base.OnStartServer();
            // rigidbody2d.position = new Vector2(1, 1);
        }

        void FixedUpdate()
        {
            // rigidbody2d.velocity = new Vector2(0, 1) * speed * Time.fixedDeltaTime;
            rigidbody2d.AddTorque(45 * Time.fixedDeltaTime);
            Debug.Log("FixedUpdate");
        }

        public void setStartPosition(Vector2 position)
        {
            rigidbody2d.position = position;
        }

        public void sayHello()
        {
            Debug.Log("HELLO!!!!!!!!!!!");
        }
    }
}