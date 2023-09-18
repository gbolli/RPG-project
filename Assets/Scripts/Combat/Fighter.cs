using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;

        private void Update()
        {
            if (target == null) return;   // if not attacking then return
            if (Vector3.Distance(transform.position, target.position) <= weaponRange) GetComponent<Mover>().Stop(); // if within attacking distance, stop
            else GetComponent<Mover>().MoveTo(target.position);  // if not within attacking distance, keep moving
        }

        public void Attack(CombatTarget combatTarget) // set target to attack
        {
            target = combatTarget.transform;
        }

        public void Cancel()  // cancel attack
        {
            target = null;
        }
    }
}
