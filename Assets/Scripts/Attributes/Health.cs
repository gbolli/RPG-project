using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] int health = 100;

        bool isDead = false;

        private void Start() {
            health = GetComponent<BaseStats>().GetHealth();
        }
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(int damage)  //  Take damage to health
        {
            health = Mathf.Max(0, health - damage);   // 0 is lowest health

            if (health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;

            GetComponent<Animator>().SetTrigger("die");   // Trigger death animation
            GetComponent<ActionScheduler>().CancelCurrentAction();  // Cancel from action scheduler
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(health);
        }

        public void RestoreFromJToken(JToken state)
        {
            health = state.ToObject<int>();

            if (health <= 0)
            {
                Die();
            }
        }
    }
}

