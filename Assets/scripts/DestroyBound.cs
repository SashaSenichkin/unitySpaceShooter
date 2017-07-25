using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBound : MonoBehaviour {
    public int maxZ;
    public int AsteroidRevard;
    private GameControlLogic MainGameLogic;
    void Start()
    {
        MainGameLogic = StaticFunctions.GetInstance();
    }
    void Update()
    {
        if (Mathf.Abs(transform.position.z) >= maxZ)
        {
            if (gameObject.tag == "Asteroid")
            {
                MainGameLogic.Score +=AsteroidRevard;
            }
            gameObject.tag = "BoundDestroy";
            Destroy(gameObject);

        }
    }
}
