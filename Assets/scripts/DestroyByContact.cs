using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public int strength;
    private int collisionCount;
    public GameObject AstExplosion;
    public GameObject PlayerExplosion;
    private GameControlLogic MainGameLogic;

    void Start() {
        MainGameLogic = StaticFunctions.GetInstance();
    }
    void OnTriggerEnter(Collider other)
    {
        if (collisionCount >= strength)
        {
            Destroy(gameObject);
            GameObject newAstExplose = Instantiate(AstExplosion, transform.position, transform.rotation); 
            Destroy(newAstExplose, 2);

        }
        else
            collisionCount++;
        Destroy(other.gameObject);
    }
}
