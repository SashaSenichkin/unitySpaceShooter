using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelControl //Presenter
    {
        public readonly LevelParams CurrentParams;
        private readonly GeneralParams GenParams;
        public int Score { get; private set; } = 0;

        private bool GameOver = false;

        public LevelControl(LevelParams currentParams)
        {
            CurrentParams = currentParams;
            GenParams = GeneralParams.Instance;
            GenParams.StartCoroutine(SpawnWaves());
            UpdateScore(0);
        }
        /*
        снимать очки за каждый выстрел
        добавлять за каждый астероид, уничтоженный границей
        возможные улучшения: добавление небольшого количества очков за уничтоженные астероиды
        ускорение перезарядки
        двойной-тройной выстрел
        укрепление брони игрока
        увеличение наград, уменьшение цен
        установка мин
        */

        public void UpdateScore(int changeValue)
        {
            Score += changeValue;
            GenParams.ViewScript.ScoreText.text = View.UI_Score + Score;
        }

        IEnumerator SpawnWaves()
        {
            while (!GameOver)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-GenParams.GameFieldHalfWidth, GenParams.GameFieldHalfWidth), 0, GenParams.GameFieldHalfHeight);
                Quaternion spawnRotation = Quaternion.identity;
                var asteroid = GenParams.AsteroidHazards[Random.Range(0, 3)];
                asteroid.transform.localScale *= CurrentParams.AsteroidsScale;

                var localRig = asteroid.GetComponent<Rigidbody>();
                var localParams = asteroid.GetComponent<AsteroidParams>();
                if (localRig != null && localParams != null)
                {
                    localRig.velocity = asteroid.transform.forward * ((-localParams.linSpeed) * (Random.value + 1)) + new Vector3(Random.Range(-10, 10) * (float)(localParams.linSpeed * 0.03), 0);
                    localRig.angularVelocity = Random.insideUnitSphere * localParams.angularSpeed;
                    GeneralParams.Instantiate(asteroid, spawnPosition, spawnRotation);
                }
                else
                {
                    Debug.LogError("asteroid spawn error");
                }

                yield return new WaitForSeconds(CurrentParams.AsteroidsSpawnSpeed);
            }
            yield break;
        }
    }
}
