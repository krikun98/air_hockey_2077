using UnityEngine;

namespace Mirror.AirHockey2077
{
    public class Obstacle : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        
        [SerializeField] private Transform[] waypoints;
        private int waypointIndex = 0;

        public override void OnStartServer()
        {
            base.OnStartServer();
            transform.position = waypoints[waypointIndex].transform.position;
            rigidbody2d.position = new Vector2(1, 1);
        }

        void FixedUpdate()
        {
            // rigidbody2d.velocity = new Vector2(0, 1) * speed * Time.fixedDeltaTime;

            // if (!isServer) return;
            // rigidbody2d.AddTorque(25 * Time.fixedDeltaTime);
            // rigidbody2d.velocity  = transform.right;

            Move();
        }

        private void Move()
        {
            if (waypointIndex <= waypoints.Length - 1)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                waypoints[waypointIndex].transform.position,
                speed * Time.deltaTime);
                if (transform.position == waypoints[waypointIndex].transform.position)
                {
                    waypointIndex += 1;
                }
            } else 
            {
                waypointIndex = 0;
            }
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