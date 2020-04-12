using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0649

namespace SpaceShooter
{
    public class Player : MonoBehaviour
    {
        private const string AxisNameVert = "Vertical";
        private const string AxisNameHor = "Horizontal";
        private const string ButtonName = "Fire1";

        [SerializeField]
        private float Tilt = 4;

        [SerializeField]
        private Transform ShotSpawn;

        [SerializeField]
        private float xSize = 6;

        [SerializeField]
        private float zSize = 8;

        private Rigidbody LocalRig;
        private AudioSource LocalAudio;
        private float NextFireCounter;

        private LevelControl MyControl;


        /// <summary>
        /// call before start!
        /// </summary>
        /// <param name="control">link to main presenter</param>
        public void Initialize(LevelControl control)
        {
            MyControl = control;
            var localDestroyInfo = GetComponent<DestroyInfo>();
            localDestroyInfo.OnCollision += (collider) =>
            {
                localDestroyInfo.Health--;
                if (localDestroyInfo.Health < 0)
                    MyControl.GameOverLogic(false);
                else
                    MyControl.UpdatePlayerLives(localDestroyInfo.Health);
            };

            MyControl.UpdatePlayerLives(localDestroyInfo.Health);
        }
        public void Update()
        {
            if (MyControl == null)
                return; //have to be initilized

            if (LocalRig == null)
                LocalRig = GetComponent<Rigidbody>();

            if (LocalAudio == null)
                LocalAudio = GetComponent<AudioSource>();

            if (Input.GetButton(ButtonName) && Time.time > NextFireCounter /*&& MyControl.Score >= MyControl.CurrentLevelParams.PlayerShotCost*/)
            {
                //MyControl.UpdateScore(-MyControl.CurrentLevelParams.PlayerShotCost); //такие вещи надо описать игроку.. иначе ощущение что выстрелы ломаются...
                NextFireCounter = Time.time + MyControl.CurrentLevelParams.PlayerFireRate;
                var boltGO = Instantiate(GeneralParams.Instance.ShotPrefab, ShotSpawn.position, ShotSpawn.rotation);
                boltGO.GetComponent<Rigidbody>().velocity = Vector3.forward * MyControl.CurrentLevelParams.BoltSpeed;
                var destroyInfo = boltGO.GetComponent<DestroyInfo>();
                destroyInfo.OnCollision += (collider) => Destroy(boltGO);
                destroyInfo.OnDestroyByBorder += () => Destroy(boltGO);

                LocalAudio.Play();
            }

            var moveHorizontal = Input.GetAxis(AxisNameHor);
            var moveVertical = Input.GetAxis(AxisNameVert);

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            LocalRig.velocity = movement * 10;

            LocalRig.position = new Vector3 //вообще не помню, зачем это... проверить.
            (
                Mathf.Clamp(LocalRig.position.x, -xSize, xSize),
                0.0f,
                Mathf.Clamp(LocalRig.position.z, -zSize, zSize)
            );
            LocalRig.rotation = Quaternion.Euler(0.0f, 0.0f, -LocalRig.velocity.x * Tilt);
        }
    }
}
