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
        private GameObject _ball;
        private GameObject _computer;

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
                SpawnComputer2();
            }
        }

        public void SpawnBall()
        {
            _ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(_ball);
        }

        private void SpawnComputer2()
        {
            SpawnBall();
            Transform start = rightRacketSpawn;
            _computer = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Computer2"), start.position, start.rotation);
            NetworkServer.Spawn(_computer);
        }

        public void IncrementScore(bool position)
        {
            keeper.IncrementScore(position);
        }

        public void DespawnBall()
        {
            // destroy ball
            if (_ball != null)
                NetworkServer.Destroy(_ball);
        }

        private void DespawnComputer2()
        {
            if (_computer != null)
                NetworkServer.Destroy(_computer);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            DespawnBall();
            DespawnComputer2();
            keeper.ZeroScores();

            // call base functionality (actually destroys the player)
            base.OnServerDisconnect(conn);
        }
    }
}
