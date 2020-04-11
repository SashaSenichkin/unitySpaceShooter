using System;

namespace SpaceShooter
{
    [Serializable]
    public class LevelParams //model
    {

        public float AsteroidsSpawnSpeed;
        public float AsteroidsScale;

        public float PlayerSpeed;
        public float PlayerShotCost;
        public float PlayerShotSpeed;

        public float BoltSpeed; //15
        public float FireRate; //0.2
        public int FireCost; // 0-1


        public int LevelScoreToFin;
    }
}
