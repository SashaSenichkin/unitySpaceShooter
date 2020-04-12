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

        private bool IsGameOver = false;

        public LevelControl(LevelParams currentParams)
        {
            CurrentParams = currentParams;
            GenParams = GeneralParams.Instance;
            GenParams.StartCoroutine(SpawnWaves());
            GenParams.ViewScript.GameOverText.enabled = false;

            var playerGO = Object.Instantiate(GenParams.PlayerScript.gameObject);
            playerGO.GetComponent<Player>().Initialize(this);
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
        public void UpdatePlayerLives(int newValue)
        {
            GenParams.ViewScript.LifeText.text = View.UI_Life + newValue;
        }
        public void GameOverLogic(bool isWin)
        {
            if (isWin)
            {
                //nextLvl
            }
            else
            {
                IsGameOver = true;
                GenParams.ViewScript.GameOverText.enabled = true;
            }
        }

        IEnumerator SpawnWaves()
        {
            while (!IsGameOver)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-GenParams.GameFieldHalfWidth, GenParams.GameFieldHalfWidth), 0, GenParams.GameFieldHalfHeight);
                Quaternion spawnRotation = Quaternion.identity;
                var asteroid = GenParams.AsteroidHazards[Random.Range(0, 3)];
                var asteroidGO = Object.Instantiate(asteroid.gameObject, spawnPosition, spawnRotation);
                asteroidGO.transform.localScale *= CurrentParams.AsteroidsScale;

                var localRig = asteroidGO.GetComponent<Rigidbody>();
                var localDestroyInfo = asteroidGO.GetComponent<DestroyInfo>();

                if (localRig != null && localDestroyInfo != null)
                {
                    localRig.velocity = GenParams.transform.forward * ((-asteroid.LinSpeed) * (Random.value + 1)) + new Vector3(Random.Range(-10, 10) * (float)(asteroid.LinSpeed * 0.03), 0);
                    localRig.angularVelocity = Random.insideUnitSphere * asteroid.AngularSpeed;
                    localDestroyInfo.OnDestroyByBorder += () => { UpdateScore(asteroid.Reward); Object.Destroy(asteroidGO); };
                    localDestroyInfo.OnCollision += (collider) =>
                    {
                        localDestroyInfo.Health--;
                        if (localDestroyInfo.Health <= 0 || collider.GetComponent<Player>() != null)
                        {
                            var explosion = Object.Instantiate(localDestroyInfo.MyExplosion, asteroidGO.transform.position, Quaternion.identity);
                            Object.Destroy(explosion, 2);
                            Object.Destroy(asteroidGO);
                        }
                    };
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
