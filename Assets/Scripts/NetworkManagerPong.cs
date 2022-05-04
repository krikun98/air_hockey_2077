using System.Collections.Generic;
using UnityEngine;

/*
    Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
    API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace Mirror.AirHockey2077
{
    // Custom NetworkManager that simply assigns the correct racket positions when
    // spawning players. The built in RoundRobin spawn method wouldn't work after
    // someone reconnects (both players would be on the same side).
    [AddComponentMenu("")]
    public class NetworkManagerPong : NetworkManager
    {
        public ScoreKeeper keeper;
        public Transform leftRacketSpawn;
        public Transform rightRacketSpawn;
        GameObject ball;
        private Ball _ballInstance;
        private GameObject _computerLeft;
        private GameObject _computerRight;
        public Computer computerInstanceLeft;
        public Computer computerInstanceRight;
        List<GameObject> obstacles;
        private int NUMBER_OF_OBSTACLES = 4;
        private List<List<Vector2>> paths;
        public const float MAX_OBSTACLE_SPEED = Constants.ObstacleSpeed;
        public const float MIN_OBSTACLE_SPEED = Constants.ObstacleSpeed/2f;
        public const float MAX_OBSTACLE_ROTATION_SPEED = Constants.RotationSpeed;
        public const float MIN_OBSTACLE_ROTATION_SPEED = Constants.RotationSpeed/3f;

        public override void Awake()
        {
            base.Awake();

            paths = new List<List<Vector2>>();

            List<Vector2> squarePath = new List<Vector2>();
            squarePath.Add(new Vector2(-6, 4));
            squarePath.Add(new Vector2(-6, -4));
            squarePath.Add(new Vector2(6, -4));
            squarePath.Add(new Vector2(6, 4));
            paths.Add(squarePath);

            List<Vector2> trianglePath = new List<Vector2>();
            trianglePath.Add(new Vector2(0, 3));
            trianglePath.Add(new Vector2(-4, -3));
            trianglePath.Add(new Vector2(4, -3));
            paths.Add(trianglePath);

            List<Vector2> horizontallPath = new List<Vector2>();
            horizontallPath.Add(new Vector2(-6, 0));
            horizontallPath.Add(new Vector2(6, 0));
            paths.Add(horizontallPath);

            List<Vector2> staticPath = new List<Vector2>();
            staticPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            paths.Add(staticPath);

            List<Vector2> randomPath = new List<Vector2>();
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            paths.Add(randomPath);
        }

        public override void OnStartServer() {
            SpawnComputer(leftRacketSpawn, "Computer2");
            SpawnComputer(rightRacketSpawn, "SmartComputer");
            SpawnBall();
            SpawnObstacles();
			base.OnStartServer();
		}

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {           
            DespawnComputers();
        	DespawnBall();
        	DespawnObstacles();
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
            
            keeper.ZeroScores();
            // SpawnObstacles();
            // spawn ball if two players
            if (numPlayers == 2)
            {
                DespawnComputers();
                SpawnBall();
                SpawnObstacles();
            }

            if (numPlayers == 1)
            {
                SpawnComputer(rightRacketSpawn, "SmartComputer");
                SpawnBall();
                SpawnObstacles();
                // SpawnSmartComputer();
//                 SpawnComputer2();
            }
        }
        
        
        
        public void SpawnComputer(Transform location, string className)
        {
            if (!(computerInstanceLeft && location == leftRacketSpawn) || !(computerInstanceRight && location == rightRacketSpawn))
            {
                var pref = spawnPrefabs.Find(prefab => prefab.name == className);
				if (location == leftRacketSpawn) {
                	_computerLeft = Instantiate(pref, location.position, location.rotation);
                	NetworkServer.Spawn(_computerLeft);
                	computerInstanceLeft = _computerLeft.GetComponent<Computer>();
				}
				else {
                	_computerRight = Instantiate(pref, location.position, location.rotation);
                	NetworkServer.Spawn(_computerRight);
                	computerInstanceRight = _computerRight.GetComponent<Computer>();
				}
            }
        }

        public void SpawnBall()
        {
            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
            _ballInstance = ball.GetComponent<Ball>();
            _ballInstance.UpdateManager(this);
            if (computerInstanceLeft)
            {
                computerInstanceLeft.UpdateBall(_ballInstance);
            }
            if (computerInstanceRight)
            {
                computerInstanceRight.UpdateBall(_ballInstance);
            }

        }

        public void IncrementScore(bool position)
        {
            keeper.IncrementScore(position);
        }

        public void DespawnBall()
        {
            // destroy ball
            if (ball != null)
                NetworkServer.Destroy(ball);
        }

        private void DespawnComputers()
        {
            if (_computerLeft != null) {
                NetworkServer.Destroy(_computerLeft);
			}
            if (_computerRight != null) {
                NetworkServer.Destroy(_computerRight);
			}
            DespawnBall();
		}

        public void SpawnObstacles()
        {
            RegenerateRandomPaths();

            obstacles = new List<GameObject>();
            int numObstacles = Random.Range(1, Constants.MaxObstacleCount);
            // int numObstacles = 4;
            Debug.Log("numObstacles=" + numObstacles);
            List<int> obstaclesNums = GenerateRandom(numObstacles, NUMBER_OF_OBSTACLES);
            List<int> pathsNums = GenerateRandomUnique(paths.Count);
            PrintList(obstaclesNums, "obstaclesNums");
            PrintList(pathsNums, "pathsNums");
            for(int i = 0; i < numObstacles; i++)
            {
                var obstacle = CreateObstacle(obstaclesNums[i] + 1, pathsNums[i]);
                obstacles.Add(obstacle);
                NetworkServer.Spawn(obstacle);
            }

            // Debug.Log("SpawnObstacles");
            // var obstacle = CreateObstacle(4, 0);
            // NetworkServer.Spawn(obstacle);
            // Debug.Log("NetworkServer.Spawn(obstacle);");
        }

        private GameObject CreateObstacle(int obstacleNum, int pathNum)
        {
            Debug.Log("CreateObstacle" + obstacleNum);
            GameObject obstacle = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Obstacle" + obstacleNum));
            Obstacle other = (Obstacle) obstacle.GetComponent(typeof(Obstacle));
            other.SetPath(paths[pathNum]);
            other.SetStartPosition(paths[pathNum][0]);
            float _speed = Random.Range(MIN_OBSTACLE_SPEED, MAX_OBSTACLE_SPEED);
            other.SetSpeed(_speed);
            Debug.Log("_speed=" + _speed);
            if (Random.value > 0.5f)
            {
                float _rotationSpeed = Random.Range(MIN_OBSTACLE_ROTATION_SPEED, MAX_OBSTACLE_ROTATION_SPEED);
                other.SetRotationSpeed(_rotationSpeed);
                Debug.Log("_rotationSpeed=" + _rotationSpeed);
            }
            return obstacle;
        }

        public void DespawnObstacles()
        {
            // destroy ball
            if (obstacles != null) {                
                foreach(GameObject obstacle in obstacles){
                    NetworkServer.Destroy(obstacle);
                }
            }
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            DespawnBall();
            DespawnComputers();
            DespawnObstacles();
            keeper.ZeroScores();

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }

        public static List<int> GenerateRandom(int count, int rangeEnd)
        {
            List<int> result = new List<int>();
            for(int j = 0; j < count; j++)
            {
                result.Add(Random.Range(0, rangeEnd));
            }
            return result;
        }

        public static List<int> GenerateRandomUnique(int count)
        {
            List<int> result = new List<int>();
            for(int j = 0; j < count; j++)
            {
                result.Add(j);
            }

            // shuffle the results:
            int i = result.Count;  
            while (i > 1)
            {  
                i--;  
                int k = Random.Range(0, i + 1);  
                int value = result[k];  
                result[k] = result[i];  
                result[i] = value;  
            }  
            return result;
        }

        private void RegenerateRandomPaths()
        {
            ClearRandomPaths();

            List<Vector2> staticPath = new List<Vector2>();
            staticPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            paths.Add(staticPath);

            List<Vector2> randomPath = new List<Vector2>();
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            randomPath.Add(new Vector2(Random.Range(-5, 5), Random.Range(-3, 3)));
            paths.Add(randomPath);
        }

        private void ClearRandomPaths()
        {
            paths.RemoveAt(paths.Count - 1);
            paths.RemoveAt(paths.Count - 1);
        }

        private static void PrintList(List<int> myList, string listName)
        {
            string result = listName + " contents: ";
            foreach (var item in myList)
            {
                result += item.ToString() + ", ";
            }
            Debug.Log(result);
        }
    }
}
