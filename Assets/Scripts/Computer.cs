using System;
using Mirror.Examples.Pong;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mirror.AirHockey2077
{
    public class Computer : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        private GameObject ball;
        private Vector2 ballPos;

        public override void OnStartServer()
        {
            base.OnStartServer();
            // only simulate ball physics on server
            rigidbody2d.simulated = true;
            
        }

        [ServerCallback]
        void Update()
        {
            if (!ball)
            {
                ball = GameObject.Find("Ball(Clone)");
            }
            Assert.IsNotNull(ball);
            
            ballPos = ball.transform.position;
            if (rigidbody2d.position.y < ballPos.y)
            {
                rigidbody2d.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
                // Vector2 dir = new Vector2(0, 1);
                // rigidbody2d.velocity = dir * speed;
            }
            
            if (transform.localPosition.y > ballPos.y)
            {
                rigidbody2d.transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
                // Vector2 dir = new Vector2(0, -1);
                // rigidbody2d.velocity = dir * speed;
            }
        } 
    }
}
