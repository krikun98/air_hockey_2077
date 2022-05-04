using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Mirror.AirHockey2077
{
    public class Computer2 : Computer 
	{       
        private GameObject _wallTop;
        private GameObject _wallBottom;
        private float _wallTopPos;
        private float _wallBottomPos;
        private float _myPosY;        
        private GameObject _ball;
        private float _ballPosY;

        protected override void AI()
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

            if (Math.Abs(_myPosY - _ballPosY) < Constants.IgnoreThreshold) {
				return;
			}
            if (_myPosY < _ballPosY && _myPosY < _wallTopPos)
            {
                Move(new Vector3(0, speed * Time.fixedDeltaTime, 0));
            }
            
            if (_myPosY > _ballPosY && _myPosY > _wallBottomPos)
            {
                Move(new Vector3(0, -speed * Time.fixedDeltaTime, 0));
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