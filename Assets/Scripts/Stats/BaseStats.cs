using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass = CharacterClass.Player;
        [SerializeField] Progression progression = null;

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
                print("Levelled Up!");
            }
        }

        public int GetStat(Stat stat) {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            // guard against race condition (ensure setting level before using)
            if (currentLevel < 1) {
                currentLevel = CalculateLevel();
            }

            return currentLevel;
        }

        public int CalculateLevel() {
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
    }
}
