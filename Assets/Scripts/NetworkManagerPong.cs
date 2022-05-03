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
        List<GameObject> obstacles;
        private int NUMBER_OF_OBSTACLES = 4;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            // add player at correct spawn position
            Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
            
            keeper.ZeroScores();

            // spawn ball if two players
            if (numPlayers == 2)
            {
                SpawnBall();
                SpawnObstacles();
            }
        }

        public void SpawnBall()
        {
            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
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

        public void SpawnObstacles()
        {
            // int numObstacles = Random.Range(1, NUMBER_OF_OBSTACLES + 1);
            Debug.Log("SpawnObstacles");
            var obstacle = CreateObstacle();
            NetworkServer.Spawn(obstacle);
            Debug.Log("NetworkServer.Spawn(obstacle);");
        }

        private GameObject CreateObstacle()
        {
            GameObject obstacle = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Obstacle4"));
            Debug.Log("Instantiate");
            Obstacle other = (Obstacle) obstacle.GetComponent(typeof(Obstacle));
            other.sayHello();
            // other.setStartPosition(new Vector2(-5, 2));
            Debug.Log("obstacle.sayHello();");
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
            DespawnObstacles();
            keeper.ZeroScores();

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
    }
}
