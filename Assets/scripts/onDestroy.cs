using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onDestroy : MonoBehaviour {
    public GameObject expolosion;
    void OnDestroy()
    {
        if (this.gameObject.tag == "BoundDestroy")
            return;
        GameObject newAstExplose = Instantiate(expolosion, transform.position, transform.rotation);
        Destroy(newAstExplose, 2);
    }
}
