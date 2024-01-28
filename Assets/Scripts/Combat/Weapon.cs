using UnityEngine;

namespace RPG.Combat {

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] int weaponDamage = 15;
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {
            if (weaponPrefab != null) {
                Transform handTransform = isRightHanded ? rightHand : leftHand;
                Instantiate(weaponPrefab, handTransform);
            }

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