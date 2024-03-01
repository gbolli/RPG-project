using System.Collections;
using System.Collections.Generic;
using RPG.UI.DamageText;
using UnityEngine;

namespace RPG.UI.DamageText {
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab = null;

        void Start()
        {
            Spawn(55);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public void Spawn(int damage) {
            if (damageTextPrefab != null) Instantiate<DamageText>(damageTextPrefab, transform);
        }
    }
}
