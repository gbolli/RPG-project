using UnityEngine;

namespace RPG.Combat {

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] int weaponDamage = 15;

        public void Spawn(Transform handTransform, Animator animator) {
            if(weaponPrefab != null) Instantiate(weaponPrefab, handTransform);
            if(weaponOverride != null) animator.runtimeAnimatorController = weaponOverride;
        }

        public float GetRange() {
            return weaponRange;
        }

        public int GetDamage() {
            return weaponDamage;
        }
    }

}