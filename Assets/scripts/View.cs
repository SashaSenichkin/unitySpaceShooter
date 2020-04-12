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
        public const string UI_LevelWin = "Level complete";
        public const string UI_LevelLose = "YOU DIED";
    }
}
