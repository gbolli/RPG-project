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
            if (target == null) return;
            if (Vector3.Distance(transform.position, target.position) <= weaponRange) GetComponent<Mover>().Stop(); 
            else GetComponent<Mover>().MoveTo(target.position);
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}
