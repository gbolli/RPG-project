using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int health = 100;

        bool isDead = false;
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(int damage)  //  Take damage to health
        {
            health = Mathf.Max(0, health - damage);   // 0 is lowest health
            if (health <= 0)
            {
                if (isDead) return;
                isDead = true;

                GetComponent<Animator>().SetTrigger("die");   // Trigger death animation
            }
        }
    }
}

