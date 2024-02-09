using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Attributes {
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        TextMeshProUGUI text;

        private void Awake() {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            text.text = health.GetHealthDisplay();
        }
    }
}
