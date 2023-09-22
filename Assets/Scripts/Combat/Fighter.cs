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

        float timeSinceLastAttack = 0f;
        Health target;

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead()) return;   // if not attacking or target dead then return

            if (Vector3.Distance(transform.position, target.transform.position) <= weaponRange)
            {
                GetComponent<Mover>().Cancel(); // if within attacking distance, stop
                AttackBehavior();
            }
            else GetComponent<Mover>().MoveTo(target.transform.position);  // if not within attacking distance, keep moving
        }

        private void AttackBehavior()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");  // Trigger attack animation and Hit() event
                timeSinceLastAttack = 0;
            }
                
        }

        public void Attack(CombatTarget combatTarget) // set target to attack
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()  // cancel attack
        {
            GetComponent<Animator>().SetTrigger("stopAttack");  // stop the attack animation
            target = null;
        }

        void Hit() // Animation Event
        {
            target.TakeDamage(weaponDamage);   // Deal damage on attack hit
        }
    }
}
