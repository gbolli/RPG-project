using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour
    {
        public event Action onLevelUp;

        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = CharacterClass.Player;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        int currentLevel = 0;

        private void Start() {
            currentLevel = CalculateLevel();

            Experience experience = GetComponent<Experience>();
            if (experience != null) {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel() {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel) {
                currentLevel = newLevel;

                LevelUpEffect();

                // delegate
                onLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public int GetStat(Stat stat)
        {
            return (int)((GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + (float)GetPercentageModifiers(stat)/100));
        }

        private int GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            // guard against race condition (ensure setting level before using)
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        private int CalculateLevel() {
            // return early if no experience (eg. enemies)
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            int currentXP = experience.GetExp();
            int totalLevels = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= totalLevels; level++)
            {
                int nextLevelExp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);

                if(nextLevelExp > currentXP) return level;
            }

            return totalLevels + 1;
        }

        private int GetAdditiveModifiers(Stat stat)
        {
            int sum = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>()) {
                foreach (int modifier in modifierProvider.GetAdditiveModifiers(stat)) {
                    sum +=  modifier;
                }
            }
            return sum;
        }

        private int GetPercentageModifiers(Stat stat)
        {
            int sum = 0;
            foreach (IModifierProvider modifierProvider in GetComponents<IModifierProvider>()) {
                foreach (int modifier in modifierProvider.GetPercentageModifiers(stat)) {
                    sum +=  modifier;
                }
            }
            print("Percentage modifier: " + sum);
            return sum;
        }
    }
}
