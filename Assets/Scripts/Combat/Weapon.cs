using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] int weaponDamage = 15;
        [SerializeField] int percentageBonus = 0;
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
            // else Debug.Log("Unarmed or Weapon needs an animator override controller" + this.name);
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, int calculatedDamage) {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
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

        public int GetPercentageBonus() {
            return percentageBonus;
        }
    }

}