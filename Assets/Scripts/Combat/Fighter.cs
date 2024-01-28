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
        //[SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        //[SerializeField] int weaponDamage = 15;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        Weapon currentWeapon = null;

        private void Start() {
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead()) return;   // if not attacking or target dead then return

            if (Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange())
            {
                GetComponent<Mover>().Cancel(); // if within attacking distance, stop
                AttackBehavior();
            }
            else GetComponent<Mover>().MoveTo(target.transform.position, 1f);  // if not within attacking distance, keep moving
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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

            if (currentWeapon.HasProjectile()) {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            } 
            else {
                target.TakeDamage(currentWeapon.GetDamage());   // Deal damage on attack hit
            }
        }

        void Shoot() // Animation Event
        {
            Hit();
        }

    }
}
