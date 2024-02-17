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
        int health = -1;
        [SerializeField] int baseHealth = 0;

        // TODO - add player base health (updated when leveling), also avoid looping through progression every frame for display.

        bool isDead = false;

        private void Start()
        {
            // ensure that this doesn't run after restoring a save
            if (health < 0) {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
            
            baseHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, int damage)  //  Take damage to health
        {
            health = Mathf.Max(0, health - damage);   // 0 is lowest health

            if (health <= 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        public string GetHealthDisplay() {
            return health + " / " + baseHealth;
            // TODO - use baseHealth variable instead of call to BaseStats (loop is expensive)
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;

            GetComponent<Animator>().SetTrigger("die");   // Trigger death animation
            GetComponent<ActionScheduler>().CancelCurrentAction();  // Cancel from action scheduler
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.Experience));
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

