using UnityEngine;

namespace Mirror.AirHockey2077
{
    public class Ball : NetworkBehaviour
    {
        public float speed = 30;
        public Rigidbody2D rigidbody2d;
        
        public NetworkManagerPong manager;
        private float _currentX;
        private float _currentY;
        private float _lastX;
        private float _lastY;
        private float _ballDx;
        private float _ballDy;
        private Line _dir = new Line(0, 1, 0);
        private Vector3 _previousPosition;

        public override void OnStartServer()
        {
            base.OnStartServer();

            // only simulate ball physics on server
            rigidbody2d.simulated = true;
            ServeBall();
        }

        void Awake()
        {
            ServeBall();
        }

        public void ServeBall()
        {
            Vector2 dirVector = Vector2.right;
            if (Random.Range(0f, 1f) < 0.5f)
            {
                dirVector = Vector2.left;
            }
            float tilt = Random.Range(0f, 0.5f);
            Debug.Log("Tilt is " + tilt);
            rigidbody2d.position = Vector2.up * 5;
            // Serve the ball from left player
            rigidbody2d.velocity = (Vector2.down * tilt + dirVector * (1-tilt))* speed;
        }

        float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
        {
            // ascii art:
            // ||  1 <- at the top of the racket
            // ||
            // ||  0 <- at the middle of the racket
            // ||
            // || -1 <- at the bottom of the racket
            return (ballPos.y - racketPos.y) / racketHeight;
        }

        // only call this on server
        [ServerCallback]
        void OnCollisionEnter2D(Collision2D col)
        {
            // Note: 'col' holds the collision information. If the
            // Ball collided with a racket, then:
            //   col.gameObject is the racket
            //   col.transform.position is the racket's position
            //   col.collider is the racket's collider

            // did we hit a racket? then we need to calculate the hit factor
            if (col.transform.GetComponent<Player>() || col.transform.GetComponent<DefaultComputer>() || col.transform.GetComponent<SmartComputer>())
            {
                // Calculate y direction via hit Factor
                float y = HitFactor(transform.position,
                                    col.transform.position,
                                    col.collider.bounds.size.y);

                // Calculate x direction via opposite collision
                float x = col.relativeVelocity.x > 0 ? 1 : -1;

                // Calculate direction, make length=1 via .normalized
                Vector2 dir = new Vector2(x, y).normalized;

                // Set Velocity with dir * speed
                rigidbody2d.velocity = dir * speed;
            }
        }
        
        public float X()
        {
            return _currentX;
        }

        public float Y()
        {
            return _currentY;
        }
        
        public float Dx()
        {
            return _ballDx;
        }

        public float Dy()
        {
            return _ballDy;
        }
        
        public void UpdatePosition()
        {
            var position = rigidbody2d.transform.position;
            _currentX = position.x;
            _currentY = position.y;
            _ballDx = _currentX - _lastX;
            _ballDy = _currentY - _lastY;
            _lastX = _currentX;
            _lastY = _currentY;
        }
        
        public void UdateManager(NetworkManagerPong m)
        {
            manager = m;
        }

        private void Update() {
            if(transform.position != _previousPosition) {
                var position = transform.position;
                _dir = new Line(position.x, position.y, _previousPosition.x, _previousPosition.y);
                _previousPosition = position;
            }
        }

        public Line Dir()
        {
            return _dir;
        }
    }
}
