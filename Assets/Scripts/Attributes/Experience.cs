using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Attributes {
    public class Experience : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] int experiencePoints = 0;


        public void GainExperience(int exp) {
            experiencePoints += exp;

            // possibly check for gain level here?
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
