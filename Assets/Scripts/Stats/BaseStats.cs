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

        public int GetHealth() {
            // Debug.Log("BaseStats call: " + characterClass + " " + startingLevel);
            return progression.GetHealth(characterClass, startingLevel);
        }

        public int GetExperienceReward() {
            return 10;
            // TODO: replace this placeholder with proper XP calc
        }
    }
}
