using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpaceShooter
{
    public class GameManager: MonoBehaviour
    {
        private void Start()
        {
            var test = new LevelParams();
            var currentLvl = new LevelControl(test);
        }
    }
}
