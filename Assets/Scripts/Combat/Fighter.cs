using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] int weaponDamage = 15;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Transform handTransform = null;
        [SerializeField] AnimatorOverrideController weaponOverride = null;

        float timeSinceLastAttack = Mathf.Infinity;
        Health target;

        private void Start() {
            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead()) return;   // if not attacking or target dead then return

            if (Vector3.Distance(transform.position, target.transform.position) <= weaponRange)
            {
                GetComponent<Mover>().Cancel(); // if within attacking distance, stop
                AttackBehavior();
            }
            else GetComponent<Mover>().MoveTo(target.transform.position, 1f);  // if not within attacking distance, keep moving
        }

        private void SpawnWeapon()
        {
            Instantiate(weaponPrefab, handTransform);
            Animator animator = GetComponent<Animator>();
            if (weaponOverride != null) animator.runtimeAnimatorController = weaponOverride;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform); // look at target when attacking

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();  // separate method
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            // ??? - suggested to reset "stopAttack" trigger   GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");  // Trigger attack animation and Hit() event
        }

        public void Attack(GameObject combatTarget) // set target to attack
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()  // cancel attack
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            // ??? - suggested to reset "attack" trigger   GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");  // stop the attack animation
        }

        public bool CanAttack(GameObject target)
        {
            return target != null && !target.GetComponent<Health>().IsDead();
        }

        void Hit() // Animation Event
        {
            if (target == null) return;

            target.TakeDamage(weaponDamage);   // Deal damage on attack hit
        }
    }
}
