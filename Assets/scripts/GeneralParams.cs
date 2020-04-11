using System;
using UnityEngine;

namespace SpaceShooter
{
    public class GeneralParams: MonoBehaviour //model
    {
        [SerializeField]
        private GameObject[] asteroidHazards;
        public GameObject[] AsteroidHazards => asteroidHazards;

        [SerializeField]
        private GameObject playerPrefab;
        public GameObject PlayerPrefab => playerPrefab;


        [SerializeField]
        private GameObject shotPrefab;
        public GameObject ShotPrefab => shotPrefab;

        [SerializeField]
        private View viewScript;
        public View ViewScript => viewScript;


        public int GameFieldHalfHeight { get; } = 16;
        public int GameFieldHalfWidth { get; } = 8;

        public static GeneralParams Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}
