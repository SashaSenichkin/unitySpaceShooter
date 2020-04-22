using System;
using System.ComponentModel;
using UnityEngine;
#pragma warning disable CS0649
namespace SpaceShooter
{
    public class GeneralParams: MonoBehaviour //model
    {
        [SerializeField]
        private AsteroidParams[] asteroidHazards;
        public AsteroidParams[] AsteroidHazards => asteroidHazards;

        [SerializeField]
        private GameObject shotPrefab;
        public GameObject ShotPrefab => shotPrefab;

        [SerializeField]
        private View viewScript;
        public View ViewScript => viewScript;

        public event Action<int> OnScoreChanged;
        private int score;
        public int Score
        {
            get { return score; }
            set { 
                score = value;
                OnScoreChanged?.Invoke(score);
            }
        }


        public int GameFieldHalfHeight { get; } = 16;
        public int GameFieldHalfWidth { get; } = 8;

        public static GeneralParams Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
