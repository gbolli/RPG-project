using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, IJsonSaveable
    {
        // public delegate void ExperienceGainedDelegate();   use Action instead
        public event Action onExperienceGained;

        [SerializeField] int experiencePoints = 0;


        public void GainExperience(int exp) {
            experiencePoints += exp;
            // call delegate
            onExperienceGained();
        }

        public int GetExp() {
            return experiencePoints; // + " / " + GetComponent<BaseStats>().GetStat(Stat.Experience);
            // TODO - use baseHealth variable instead of call to BaseStats (loop is expensive)
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(experiencePoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            experiencePoints = state.ToObject<int>();
        }
    }
}
