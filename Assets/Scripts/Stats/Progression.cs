using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]

    public class Progression : ScriptableObject {

        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, int[]>> lookupTable = null;

        public int GetStat(Stat stat, CharacterClass characterClass, int level) {
            BuildLookupTable();

            int[] levels = lookupTable[characterClass][stat];
            // guard against out of bounds level
            return levels.Length > level ? levels[level - 1] : 0;
        }

        private void BuildLookupTable()
        {
            if (lookupTable != null) return;
            Debug.Log("Building LookupTable");
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, int[]>>();
            var newStat = new Dictionary<Stat, int[]>();

            foreach (ProgressionCharacterClass PCC in characterClasses) {
                newStat.Clear();

                foreach (ProgressionStat PS in PCC.stats) {
                    newStat.Add(PS.stat, PS.levels); 
                }

                lookupTable.Add(PCC.characterClass, newStat);
            }
       
            PrintLookupTable();
        }

        public int GetLevels(Stat stat, CharacterClass characterClass) {
            BuildLookupTable();
            
            return lookupTable[characterClass][stat].Length;
        }

        private void PrintLookupTable() {
            Debug.Log("Printing Lookup Table");
            foreach (var PCC in lookupTable) {
                Debug.Log(PCC.Key);
                foreach (var stat in PCC.Value) {
                    Debug.Log(stat.Key);
                    foreach (int level in stat.Value) {
                        Debug.Log(level);
                    }
                }
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat {
            public Stat stat;
            public int[] levels;
        }
    }
}