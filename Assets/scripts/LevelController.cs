using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelController

    {
        public readonly LevelParams CurrentLevelParams;
        private readonly GeneralParams GenParams;

        public event System.Action<bool> OnLevelFinished;

        private bool IsGameOver = false;
        private float NextFireCounter;

        public LevelController(LevelParams currentParams)
        {
            CurrentLevelParams = currentParams;
            GenParams = GeneralParams.Instance;
            GenParams.StartCoroutine(SpawnWaves());

            GenParams.ViewScript.Initialize(this);
            UpdateScore(0);
        }
        /*
        снимать очки за каждый выстрел
        возможные улучшения: добавление небольшого количества очков за уничтоженные астероиды
        ускорение перезарядки
        двойной-тройной выстрел
        укрепление брони игрока
        увеличение наград, уменьшение цен
        установка мин
        */

        public void UpdateScore(int changeValue)
        {
            GenParams.Score += changeValue;
            if (GenParams.Score >= CurrentLevelParams.LevelScoreToFin)
                OnLevelFinished(true);
        }

        public void GameOverLogic(bool? isWin)
        {
            if (IsGameOver)
                OnLevelFinished?.Invoke(false);

            IsGameOver = true;
            GenParams.StartCoroutine(WaitAndFinishGame(isWin == true));
        }

        IEnumerator WaitAndFinishGame(bool isWin)
        {
            yield return new WaitForSeconds(2);
            OnLevelFinished?.Invoke(isWin);
        }

        IEnumerator SpawnWaves()
        {
            while (!IsGameOver)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-GenParams.GameFieldHalfWidth, GenParams.GameFieldHalfWidth), 0, GenParams.GameFieldHalfHeight);
                Quaternion spawnRotation = Quaternion.identity;
                var asteroid = GenParams.AsteroidHazards[Random.Range(0, 3)];
                var asteroidGO = Object.Instantiate(asteroid.gameObject, spawnPosition, spawnRotation, GenParams.transform);
                asteroidGO.transform.localScale *= CurrentLevelParams.AsteroidsScale;

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

                yield return new WaitForSeconds(CurrentLevelParams.AsteroidsSpawnSpeed);
            }
            yield break;
        }

        public void PlayerShot(Transform shopSpawn, AudioSource spaceshipAudio)
        {
            if (Time.time > NextFireCounter /*&& MyControl.Score >= MyControl.CurrentLevelParams.PlayerShotCost*/)
            {
                //MyControl.UpdateScore(-MyControl.CurrentLevelParams.PlayerShotCost); //такие вещи надо описать игроку.. иначе ощущение что выстрелы ломаются...
                NextFireCounter = Time.time + CurrentLevelParams.PlayerFireRate;
                var boltGO = Object.Instantiate(GenParams.ShotPrefab, shopSpawn.position, shopSpawn.rotation);
                boltGO.GetComponent<Rigidbody>().velocity = Vector3.forward * CurrentLevelParams.BoltSpeed;
                var destroyInfo = boltGO.GetComponent<DestroyInfo>();
                destroyInfo.OnCollision += (collider) => Object.Destroy(boltGO);
                destroyInfo.OnDestroyByBorder += () => Object.Destroy(boltGO);

                spaceshipAudio.Play();
            }
        }

    }
}
