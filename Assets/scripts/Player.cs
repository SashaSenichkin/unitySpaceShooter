using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class Player : MonoBehaviour {
    public float speed;
    public float tilt;
    public Boundary boundary;
    public int FireCost;
    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;
    private GameControlLogic MainGameLogic;

    void Start()
    {
        MainGameLogic = StaticFunctions.GetInstance();
    }
    public void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire && MainGameLogic.Score > FireCost)
        {
            MainGameLogic.Score -= FireCost;
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement*10;

        GetComponent<Rigidbody>().position = new Vector3
(
    Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
    0.0f,
    Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
);
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, -GetComponent<Rigidbody>().velocity.x * tilt);
    }
}
