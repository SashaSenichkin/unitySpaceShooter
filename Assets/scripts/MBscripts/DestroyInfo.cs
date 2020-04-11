using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class DestroyInfo : MonoBehaviour
    {
        public int Health;
        public GameObject MyExplosion;

        public event Action OnDestroyByBorder;
        public event Action OnCollision;

        void Update()
        {
            if (Mathf.Abs(transform.position.z) >= GeneralParams.Instance.GameFieldHalfHeight && OnDestroyByBorder != null)
                OnDestroyByBorder();
        }

        void OnTriggerEnter(Collider other)
        {
            if (OnCollision != null)
                OnCollision();
        }
    }
}