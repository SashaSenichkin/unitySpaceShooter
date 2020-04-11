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
        private float Tilt = 2;

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
            LocalRig = GetComponent<Rigidbody>();
            LocalAudio = GetComponent<AudioSource>();
        }
        public void Update()
        {
            if (Input.GetButton(ButtonName) && Time.time > NextFireCounter && MyControl?.Score > MyControl.CurrentParams.FireCost)
            {
                MyControl.UpdateScore(-MyControl.CurrentParams.FireCost);
                NextFireCounter = Time.time + MyControl.CurrentParams.FireRate;
                var bolt = Instantiate(GeneralParams.Instance.ShotPrefab, ShotSpawn.position, ShotSpawn.rotation);
                bolt.GetComponent<Rigidbody>().velocity = Vector3.forward * MyControl.CurrentParams.BoltSpeed;
                LocalAudio.Play();
            }

            float moveHorizontal = Input.GetAxis(AxisNameHor);
            float moveVertical = Input.GetAxis(AxisNameVert);

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
