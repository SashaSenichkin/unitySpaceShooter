using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{

    public float thumble;
    public float speed;
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * (speed * (Random.value + 1));
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * thumble;
        GetComponent<Rigidbody>().velocity += new Vector3(Random.Range(-10,10) * (float)(speed * 0.03), 0);
    }
}
