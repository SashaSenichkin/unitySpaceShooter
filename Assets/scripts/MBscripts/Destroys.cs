using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroys : MonoBehaviour 
{
    public int Health;
    public GameObject MyExplosion;
    public int AsteroidReward;

    private int? maxZ = null;
    private int CollisionsCount;

    private IEnumerator Start()
    {
        if (GetComponent<Player>() != null)
        {
            while (GameControlLogic.Instance == null)
                yield return new WaitForEndOfFrame();

            GameControlLogic.Instance?.UpdateLives(Health);
        }

    }

    void Update()
    {
        if (maxZ == null)
        {
            maxZ = GameControlLogic.Instance?.AsteroidStartPoint;
            return; //wait for initialization
        }

        if (Mathf.Abs(transform.position.z) >= maxZ)
        {
            if (GetComponent<AsteroidMover>() != null && GameControlLogic.Instance != null)
                GameControlLogic.Instance.Score += AsteroidReward;

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (CollisionsCount >= Health || other.gameObject.GetComponent<Player>() != null)
        {
            Destroy(gameObject);
            if (MyExplosion != null)
            {
                GameObject newAstExplose = Instantiate(MyExplosion, transform.position, transform.rotation);
                Destroy(newAstExplose, 2);
            }
        }
        else
        {
            if (GetComponent<Player>() != null)
                GameControlLogic.Instance?.UpdateLives(Health - CollisionsCount);

            CollisionsCount++;
        }
    }
}
