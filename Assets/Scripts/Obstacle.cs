using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.AirHockey2077
{
    public class Obstacle : NetworkBehaviour
    {
        public float speed = 10;
        public float _rotationSpeed = 0;
        public Rigidbody2D rigidbody2d;

        private List<Vector2> path;
        private int waypointIndex = 0;
        private int direction = 1;
        private int curPathLen = 0;

        public override void OnStartServer()
        {
            base.OnStartServer();
        }

        void FixedUpdate()
        {
            // rigidbody2d.velocity = new Vector2(0, 1) * speed * Time.fixedDeltaTime;
            // if (!isServer) return;
            // rigidbody2d.velocity  = transform.right;
            // rigidbody2d.AddTorque(45 * Time.fixedDeltaTime);
            if (isServer)
                Move();
        }

        private void Move()
        {
            transform.Rotate(0, 0, _rotationSpeed * Time.fixedDeltaTime);

            // if (waypointIndex <= path.Count - 1)
            if (curPathLen <= path.Count)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                path[waypointIndex],
                speed * Time.deltaTime);
                if (transform.position == (Vector3)path[waypointIndex])
                {
                    // waypointIndex += 1;
                    NextWaypointIndex();
                    curPathLen += 1;
                }
            } else 
            {
                // waypointIndex = 0;
                ChangeDirection();
                ResetWaypointIndex();
                ResetCurPathLen();
            }
        }

        private void ResetWaypointIndex()
        {
            if (direction < 0) {
                waypointIndex = path.Count - 1;
                return;
            }
            waypointIndex = 0;

            // Debug.Log("waypointIndex=" + waypointIndex);
        }

        private void ResetCurPathLen()
        {
            if (direction < 0) {
                curPathLen = 1;
                return;
            }
            curPathLen = 0;
        }

        private void ChangeDirection()
        {
            direction *= -1;
        }

        private void NextWaypointIndex()
        {
            waypointIndex += direction;
            if (waypointIndex == path.Count)
            {
                waypointIndex = 0;
            }
            if (waypointIndex < 0)
            {
                waypointIndex = path.Count - 1;
            }
        }

        private bool CheckEndOfPath()
        {
            if (direction < 0)
            {
                return waypointIndex >= 0;
            }

            return waypointIndex <= path.Count;
        }

        public void SetPath(List<Vector2> newPath)
        {
            path = newPath;
        }

        public void SetStartPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetRotationSpeed(float rotationSpeed)
        {
            _rotationSpeed = rotationSpeed;
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void sayHello()
        {
            Debug.Log("HELLO!!!!!!!!!!!");
        }
    }
}