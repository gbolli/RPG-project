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

        public int GetStat(Stat stat) {
            int pStat = progression.GetStat(stat, characterClass, GetLevel());
            Debug.Log("Calling GetStat() from BaseStats for " + this.name + " " + stat + " " + characterClass + " " + pStat);
            return pStat;
        }

        public int GetLevel() {
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
            Debug.Log(this.name + " is level: " + totalLevels + 1);
            return totalLevels + 1;
        }
    }
}
