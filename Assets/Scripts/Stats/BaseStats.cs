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
            Debug.Log($"{this.name} currentXP: {currentXP}");
            for (int level = 1; level <= totalLevels; level++)
            {
                int nextLevelExp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                Debug.Log($"Next Level Exp: {nextLevelExp}");
                Debug.Log(this.name + " Iterating level: " + level);
                if(nextLevelExp > currentXP) {
                    Debug.Log(this.name + " Returning level: " + level);
                    return level;
                }
            }
            Debug.Log(this.name + "Exp over limit, level: " + (totalLevels + 1));
            return totalLevels + 1;
        }
    }
}
