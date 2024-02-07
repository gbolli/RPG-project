using System;
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

        // String reference for FindByType<>
        const string weaponName = "Weapon";


        public void Spawn(Transform rightHand, Transform leftHand, Animator animator) {

            DestroyOldWeapon(rightHand, leftHand);
            
            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(weaponPrefab, handTransform);
                weapon.name = weaponName;
            }
            // replace animator controller with weapon override
            if (weaponOverride != null) animator.runtimeAnimatorController = weaponOverride;
            else Debug.Log("Weapon needs an animator override controller");
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName) ?? leftHand.Find(weaponName);
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target) {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
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