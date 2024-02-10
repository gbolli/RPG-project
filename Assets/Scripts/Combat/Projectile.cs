using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
    public class Projectile : MonoBehaviour
    {
        Health target = null;
        [SerializeField] float speed = 5;
        [SerializeField] bool isHoming = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 8f;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] GameObject[] destroyOnHit = null;

        int damage = 0;
        GameObject instigator = null;

        private void Start() {
            transform.LookAt(GetAimLocation());
        }
       
        void Update()
        {
            if (target == null) return;
            if (isHoming && !target.IsDead()) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator, int damage) {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target || target.IsDead()) return;
            // stop projectile
            speed = 0;
            // apply damage to enemy
            if (hitEffect != null) Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            other.GetComponent<Health>().TakeDamage(instigator, damage);
            // destroy parts on hit
            foreach (GameObject toDestroy in destroyOnHit) {
                Destroy(toDestroy);
            }
            // destroy completely after time (lifeAfterImpact)
            Destroy(gameObject, lifeAfterImpact);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();

            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + (Vector3.up * targetCapsule.height / 1.5f);
        }
    }
}
