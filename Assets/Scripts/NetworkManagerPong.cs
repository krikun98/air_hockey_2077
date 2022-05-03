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
        private GameObject _computer;
        public Computer computerInstance;

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
            }
            
            if (numPlayers == 1)
            {
                // SpawnDefaultComputer();
                SpawnSmartComputer();
            }
        }
        
        private void SpawnDefaultComputer()
        {
            var pref = spawnPrefabs.Find(prefab => prefab.name == "DefaultComputer");
            _computer = Instantiate(pref);
            NetworkServer.Spawn(_computer);
            computerInstance = _computer.GetComponent<DefaultComputer>();
            SpawnBall();
        }
        
        private void SpawnSmartComputer()
        {
            var pref = spawnPrefabs.Find(prefab => prefab.name == "SmartComputer");
            _computer = Instantiate(pref);
            NetworkServer.Spawn(_computer);
            computerInstance = _computer.GetComponent<SmartComputer>();
            SpawnBall();
        }

        public void SpawnBall()
        {
            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
            _ballInstance = ball.GetComponent<Ball>();
            _ballInstance.UdateManager(this);
            if (computerInstance)
            {
                computerInstance.UpdateBall(_ballInstance);
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
        
        private void DespawnComputer()
        {
            if (_computer != null)
                NetworkServer.Destroy(_computer);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            DespawnBall();
            DespawnComputer();
            keeper.ZeroScores();

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
    }
}
