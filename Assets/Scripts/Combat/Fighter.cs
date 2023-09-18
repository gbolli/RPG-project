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
            bool isInRange = Vector3.Distance(transform.position, target.position) <= weaponRange;
            if (isInRange) GetComponent<Mover>().Stop();
            else if (target != null) GetComponent<Mover>().MoveTo(target.position);
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }
    }
}
