using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes {
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience exp;
        TextMeshProUGUI text;

        private void Awake() {
            exp = GameObject.FindWithTag("Player").GetComponent<Experience>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            text.text = exp.GetExpDisplay();
        }
    }
}