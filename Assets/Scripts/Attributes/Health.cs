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

        // TODO - add player base health (updated when leveling), also avoid looping through progression every frame for display.

        bool isDead = false;

        private void Start() {
            // Debug.Log("Calling GetHealth() for " + this.name + " id: " + this.GetInstanceID());
            health = GetComponent<BaseStats>().GetHealth();
            
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
            return health + " / " + GetComponent<BaseStats>().GetHealth();
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
            
            experience.GainExperience(GetComponent<BaseStats>().GetExperienceReward());
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

