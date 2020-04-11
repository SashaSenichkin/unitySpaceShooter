using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts
{
    public class GameManager: MonoBehaviour
    {
        [Serializable]
        class LevelParams
        {
            public float AsteroidsSpawnSpeed;
            public float AsteroidsScale;

            public float PlayerSpeed;
            public float PlayerShotCost;
            public float PlayerShotSpeed;


        }
    }
}
