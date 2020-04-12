using System;

namespace SpaceShooter
{
    [Serializable]
    public class LevelParams //model
    {
        public float AsteroidsSpawnSpeed = 0.5f; 
        public float AsteroidsScale = 1;

        public float PlayerSpeed = 10;
        public int PlayerShotCost = 0; 
        public float PlayerFireRate = 0.2f;

        public float BoltSpeed = 15;

        public int LevelScoreToFin = 10;
        public bool IsLevelOpened = false;
        public LevelParams NextLevel;
    }
}
