using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMover : MonoBehaviour {

    public float angularSpeed;
    public float linSpeed;
    public float scaleMult = 1;
    // Use this for initialization
    void Start () {
        var localRig = GetComponent<Rigidbody>();
        localRig.velocity = transform.forward * ((-linSpeed) * (Random.value + 1)) + new Vector3(Random.Range(-10, 10) * (float)(linSpeed * 0.03), 0);
        localRig.angularVelocity = Random.insideUnitSphere * angularSpeed;
        transform.localScale *= scaleMult;
    }
}
