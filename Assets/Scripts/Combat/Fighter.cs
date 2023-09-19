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

        Transform target;

        private void Update()
        {
            if (target == null) return;   // if not attacking then return
            if (Vector3.Distance(transform.position, target.position) <= weaponRange)
            {
                GetComponent<Mover>().Cancel(); // if within attacking distance, stop
                AttackBehavior();
            }
            else GetComponent<Mover>().MoveTo(target.position);  // if not within attacking distance, keep moving
        }

        private void AttackBehavior()
        {
            GetComponent<Animator>().SetTrigger("attack");  // Trigger attack animation
        }

        public void Attack(CombatTarget combatTarget) // set target to attack
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()  // cancel attack
        {
            target = null;
        }

        void Hit() // Animation Event
        {
            print("I hit you!");
        }
    }
}
