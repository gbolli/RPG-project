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
        [SerializeField] bool isHoming = false;
        int damage = 0;

        private void Start() {
            transform.LookAt(GetAimLocation());
        }
       
        void Update()
        {
            if (target == null || target.IsDead()) return;
            if (isHoming) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, int damage) {
            this.target = target;
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target || target.IsDead()) return;
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
