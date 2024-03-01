using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using Newtonsoft.Json.Linq;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, IJsonSaveable
    {
        // LazyValue is custom initializer package from GameDevTV to ensure initialization before use
        LazyValue<int> health;
        [SerializeField] int baseHealth = 0;
        [SerializeField] UnityEvent takeDamage;

        // TODO - add player base health (updated when leveling), also avoid looping through progression every frame for display.

        bool isDead = false;

        private void Awake() {
            health = new LazyValue<int>(GetInitialHealth);
        }

        private void Start()
        {   
            health.ForceInit();
            baseHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void OnEnable() {
            GetComponent<BaseStats>().onLevelUp += LevelUpHealth;
        }

        private void OnDisable() {
            GetComponent<BaseStats>().onLevelUp -= LevelUpHealth;
        }

        private int GetInitialHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void LevelUpHealth()
        {
            // update health for new level
            baseHealth = GetComponent<BaseStats>().GetStat(Stat.Health);

            RegenerateFullHealth();
        }

        private void RegenerateFullHealth()
        {
            health.value = baseHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, int damage)  //  Take damage to health
        {
            print(gameObject.name + " took damage: " + damage);
    
            health.value = Mathf.Max(0, health.value - damage);   // 0 is lowest health

            takeDamage.Invoke();
            
            if (health.value <= 0)
            {
                AwardExperience(instigator);
                Die();
            }
        }

        public string GetHealthDisplay() {
            return health.value + " / " + baseHealth;
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
            return JToken.FromObject(health.value);
        }

        public void RestoreFromJToken(JToken state)
        {
            health.value = state.ToObject<int>();

            if (health.value <= 0)
            {
                Die();
            }
        }
    }
}

