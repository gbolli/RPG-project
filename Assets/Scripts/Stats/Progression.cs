using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]

    public class Progression : ScriptableObject {

        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public int GetStat(Stat stat, CharacterClass characterClass, int level) {
            foreach (ProgressionCharacterClass PCC in characterClasses) {
                if (PCC.characterClass != characterClass) continue;
                foreach (ProgressionStat PS in PCC.stats) {
                    if (PS.stat != stat) continue;
                    // guard against out of range level.  TODO: Should stop loop instead of continue?
                    if (PS.levels.Length < level) continue;
                    return PS.levels[level - 1];
                }
            }
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            // public int[] health = null;
            // public int[] damage = null;
        }

        [System.Serializable]
        class ProgressionStat {
            public Stat stat;
            public int[] levels;
        }
    }
}