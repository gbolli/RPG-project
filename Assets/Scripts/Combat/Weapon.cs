using RPG.Core;
using UnityEngine;

namespace RPG.Combat {

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] int weaponDamage = 15;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {
            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                Instantiate(weaponPrefab, handTransform);
            }

            if (weaponOverride != null) animator.runtimeAnimatorController = weaponOverride;
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target) {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }

        public bool HasProjectile() {
            return projectile != null;
        }

        public float GetRange() {
            return weaponRange;
        }

        public int GetDamage() {
            return weaponDamage;
        }
    }

}