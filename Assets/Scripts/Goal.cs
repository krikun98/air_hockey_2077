using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mirror.AirHockey2077
{
    public class Goal : MonoBehaviour
    {
        public NetworkManagerPong manager;
        // False for left, true for right
        public bool position = false;
        // Start is called before the first frame update

        [ServerCallback]
        void OnTriggerExit2D(Collider2D col)
        {
            manager.DespawnBall(); 
            manager.DespawnObstacles();
            manager.IncrementScore(position);
            manager.SpawnObstacles();
            manager.SpawnBall();
        }
    }
}
