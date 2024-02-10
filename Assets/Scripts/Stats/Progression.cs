using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]

    public class Progression : ScriptableObject {

        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public int GetHealth(CharacterClass characterClass, int level) {
            // Debug.Log(characterClasses[0]);
            foreach (ProgressionCharacterClass PCC in characterClasses) {
                // Debug.Log("Checking: " + PCC.characterClass + " " + level);
                if (PCC.characterClass == characterClass) {
                    // Debug.Log("Applying " + PCC.characterClass + " health of " + PCC.health[level - 1]);
                    // return PCC.health[level - 1];
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