using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class Player : MonoBehaviour {
    public float Speed;
    public float Tilt;
    public int FireCost;
    public float FireRate;
    public float BoltSpeed = 15;

    [SerializeField]
    private GameObject Shot;
    [SerializeField]
    public Transform ShotSpawn;
    [SerializeField]
    public Boundary Boundary;

    Rigidbody LocalRig;
    AudioSource LocalAudio;

    private float NextFire;

    private void Start()
    {
        LocalRig = GetComponent<Rigidbody>();
        LocalAudio = GetComponent<AudioSource>();
    }

    public void Update()
    {
        var mainLogic = GameControlLogic.Instance;
        if (Input.GetButton("Fire1") && Time.time > NextFire && mainLogic?.Score > FireCost)
        {
            mainLogic.Score -= FireCost;
            NextFire = Time.time + FireRate;
            var bolt = Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
            bolt.GetComponent<Rigidbody>().velocity = Vector3.forward * BoltSpeed;
            LocalAudio.Play();
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        LocalRig.velocity = movement * 10;

        LocalRig.position = new Vector3
        (
            Mathf.Clamp(LocalRig.position.x, Boundary.xMin, Boundary.xMax),
            0.0f,
            Mathf.Clamp(LocalRig.position.z, Boundary.zMin, Boundary.zMax)
        );
        LocalRig.rotation = Quaternion.Euler(0.0f, 0.0f, -LocalRig.velocity.x * Tilt);
    }
}
