using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private float xSize = 4;

        [SerializeField]
        private float zSize = 6;

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
        }
        public void Update()
        {
            if (LocalRig == null)
                LocalRig = GetComponent<Rigidbody>();

            if (LocalAudio == null)
                LocalAudio = GetComponent<AudioSource>();

            if (Input.GetButton(ButtonName) && Time.time > NextFireCounter && MyControl?.Score > MyControl.CurrentParams.PlayerShotCost)
            {
                MyControl.UpdateScore(-MyControl.CurrentParams.PlayerShotCost);
                NextFireCounter = Time.time + MyControl.CurrentParams.PlayerFireRate;
                var bolt = Instantiate(GeneralParams.Instance.ShotPrefab, ShotSpawn.position, ShotSpawn.rotation);
                bolt.GetComponent<Rigidbody>().velocity = Vector3.forward * MyControl.CurrentParams.BoltSpeed;
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
