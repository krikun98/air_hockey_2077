using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Mirror.AirHockey2077
{
    public class ScoreKeeper : NetworkBehaviour
    {
        public TextMeshProUGUI scoreText;
        
        [SyncVar]
        private int left = 0;
        
        [SyncVar]
        private int right = 0;

        // Start is called before the first frame update
        void Start()
        {
            scoreText.text = "Left:   " + left + "\nRight:  " + right;
        }

        public void IncrementScore(bool position)
        {
            if (position)
            {
                right++;
            }
            else
            {
                left++;
            }
        }

        public void ZeroScores()
        {
            left = 0;
            right = 0;
        }

        // Update is called once per frame
        void Update()
        {
            scoreText.text = "Left:   " + left + "\nRight:  " + right;
        }
    }
}
