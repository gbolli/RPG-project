using System.Collections;
using System.Collections.Generic;
using RPG.Combat;   // To get fighter component
using TMPro;
using UnityEngine;

namespace RPG.Attributes {
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Health health;
        TextMeshProUGUI text;

        private void Awake() {
            
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            // get enemy health component
            health = GameObject.FindWithTag("Player").GetComponent<Fighter>().GetTarget();
            // display enemy health / base health
            text.text = (health != null) ? health.GetHealthDisplay() : "No enemy";
        }
    }
}
