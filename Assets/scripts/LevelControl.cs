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

            GenParams.PlayerScript.Initialize(this);
            GameObject.Instantiate(GenParams.PlayerScript.gameObject);

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
                asteroid.transform.localScale *= CurrentParams.AsteroidsScale;

                var localRig = asteroid.GetComponent<Rigidbody>();
                var localDestroyInfo = asteroid.GetComponent<DestroyInfo>();

                if (localRig != null && localDestroyInfo != null)
                {
                    localRig.velocity = GenParams.transform.forward * ((-asteroid.LinSpeed) * (Random.value + 1)) + new Vector3(Random.Range(-10, 10) * (float)(asteroid.LinSpeed * 0.03), 0);
                    localRig.angularVelocity = Random.insideUnitSphere * asteroid.AngularSpeed;
                    localDestroyInfo.OnDestroyByBorder += () => { UpdateScore(asteroid.Reward); GameObject.Destroy(asteroid); };
                    localDestroyInfo.OnCollision += () => 
                        { 
                            localDestroyInfo.Health--;
                            if (localDestroyInfo.Health < 0)
                                GameOverLogic(false);
                            else
                                UpdatePlayerLives(localDestroyInfo.Health);
                        };

                    GeneralParams.Instantiate(asteroid.gameObject, spawnPosition, spawnRotation);
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
