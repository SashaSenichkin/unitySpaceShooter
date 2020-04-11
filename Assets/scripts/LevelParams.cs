using System;

namespace SpaceShooter
{
    [Serializable]
    public class LevelParams //model
    {
        public float AsteroidsSpawnSpeed = 0.5f; //0.5
        public float AsteroidsScale = 1; //1

        public float PlayerSpeed = 10; //10
        public int PlayerShotCost = 0; //0-1
        public float PlayerFireRate = 0.2f; //0.2

        public float BoltSpeed = 15; //15

        public int LevelScoreToFin = 50;
    }
}
