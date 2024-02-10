using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes {
    public class Experience : MonoBehaviour
    {
        [SerializeField] int experiencePoints = 0;

        public void GainExperience(int exp) {
            experiencePoints += exp;

            // possibly check for gain level here?
        }
    }
}
