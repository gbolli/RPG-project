using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText {
    public class DamageText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI damageText;

        public void SetValue(int damage) {
            damageText.text = damage.ToString();
        }
    }
}
