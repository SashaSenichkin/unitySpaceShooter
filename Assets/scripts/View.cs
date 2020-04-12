using System;
using UnityEngine;

namespace SpaceShooter
{
    public class View : MonoBehaviour
    {
        public UnityEngine.UI.Text ScoreText;
        public UnityEngine.UI.Text LifeText;
        public UnityEngine.UI.Text GameOverText;
        public UnityEngine.UI.Button ExitToMenu;


        public const string UI_Score = "Score: {0} of {1}";
        public const string UI_Life = "Lives: ";
        public const string UI_LevelWin = "Level completed";
        public const string UI_LevelLose = "YOU DIED";
    }
}
