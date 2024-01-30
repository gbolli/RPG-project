using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour
    {
        Health target = null;
        [SerializeField] float speed = 5;
        int damage = 0;
       
        void Update()
        {
            if (target == null) return;
            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, int damage) {
            this.target = target;
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target) return;
            // apply damage to enemy
            other.GetComponent<Health>().TakeDamage(damage);
            // destroy arrow?   or leave stuck to enemy
            Destroy(gameObject);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + (Vector3.up * targetCapsule.height / 1.5f);
        }
    }
}
