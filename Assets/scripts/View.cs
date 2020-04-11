using System;
using UnityEngine;

namespace SpaceShooter
{
    public class View : MonoBehaviour
    {
        public UnityEngine.UI.Text ScoreText;
        public UnityEngine.UI.Text LifeText;
        public UnityEngine.UI.Text GameOverText;

        public const string UI_Score = "Score: ";
        public const string UI_Life = "Lives: ";

        private void Start()
        {
            GameOverText.enabled = false;
        }
        private void UpdateScore()
        {
            //ScoreText.text = UI_Score + Score;
        }

        public void UpdateLives(int lifeNum)
        {
            LifeText.text = UI_Life + lifeNum;
        }
        public void SetTextGameOver()
        {
            GameOverText.enabled = true;
        }
    }
}
