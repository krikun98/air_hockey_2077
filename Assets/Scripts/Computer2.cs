using UnityEngine;
using UnityEngine.Assertions;

namespace Mirror.AirHockey2077
{
    public class Computer2 : NetworkBehaviour
    {
        public float speed = 10;
        
        private GameObject _wallTop;
        private GameObject _wallBottom;
        private float _wallTopPos;
        private float _wallBottomPos;
        
        public Rigidbody2D rigidbody2d;
        private float _myPosY;
        
        private GameObject _ball;
        private float _ballPosY;

        public override void OnStartServer()
        {
            base.OnStartServer();
            // only simulate ball physics on server
            rigidbody2d.simulated = true;
        }

        [ServerCallback]
        void Update()
        {
            if (!_ball)
            {
                FindBall();
            }

            if (!_wallTop)
            {
                FindWallTop();
            }
            
            if (!_wallBottom)
            {
                FindWallBottom();
            }

            _ballPosY = _ball.transform.position.y;
            _myPosY = rigidbody2d.position.y;

            
            if (_myPosY < _ballPosY && _myPosY < _wallTopPos)
            {
                rigidbody2d.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
            }
            
            if (_myPosY > _ballPosY && _myPosY > _wallBottomPos)
            {
                rigidbody2d.transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
            }
        }

        private void FindBall()
        {
            _ball = GameObject.Find("Ball(Clone)");
            Assert.IsNotNull(_ball);
        }

        private void FindWallTop()
        {
            _wallTop = GameObject.Find("WallTop");
            Assert.IsNotNull(_wallTop);
            _wallTopPos = _wallTop.transform.position.y - _wallTop.GetComponent<BoxCollider2D>().bounds.size.y * 10;
        }   


        private void FindWallBottom()
        {
            _wallBottom = GameObject.Find("WallBottom");
            Assert.IsNotNull(_wallBottom);
            _wallBottomPos = _wallBottom.transform.position.y + _wallTop.GetComponent<BoxCollider2D>().bounds.size.y * 10;
        }
    }
}