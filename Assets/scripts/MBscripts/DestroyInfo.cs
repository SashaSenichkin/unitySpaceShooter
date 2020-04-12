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
        public event Action<Collider> OnCollision;

        void Update()
        {
            if (Mathf.Abs(transform.position.z) > GeneralParams.Instance.GameFieldHalfHeight + 1)
                OnDestroyByBorder?.Invoke();
        }

        void OnTriggerEnter(Collider other)
        {
            OnCollision?.Invoke(other);
        }
    }
}