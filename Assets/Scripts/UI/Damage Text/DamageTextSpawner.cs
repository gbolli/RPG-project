using System.Collections;
using System.Collections.Generic;
using RPG.UI.DamageText;
using UnityEngine;

namespace RPG.UI.DamageText {
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;
        
        public void Spawn(int damage) {
            DamageText instance = Instantiate<DamageText>(damageTextPrefab, transform);
        }
    }
}
