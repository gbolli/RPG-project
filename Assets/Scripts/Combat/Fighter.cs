using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, IJsonSaveable, IModifierProvider
    {
        //[SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        //[SerializeField] int weaponDamage = 15;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;
        Health target;
        Weapon currentWeapon = null;

        private void Start() {
            if (currentWeapon == null) EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead()) return;   // if not attacking or target dead then return

            if (Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetRange())
            {
                GetComponent<Mover>().Cancel(); // if within attacking distance, stop
                AttackBehavior();
            }
            else GetComponent<Mover>().MoveTo(target.transform.position, 1f);  // if not within attacking distance, keep moving
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget() {
            return target;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform); // look at target when attacking

            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();  // separate method
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            // ??? - suggested to reset "stopAttack" trigger   GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");  // Trigger attack animation and Hit() event
        }

        public void Attack(GameObject combatTarget) // set target to attack
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()  // cancel attack
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            // ??? - suggested to reset "attack" trigger   GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");  // stop the attack animation
        }

        public bool CanAttack(GameObject target)
        {
            return target != null && !target.GetComponent<Health>().IsDead();
        }

        public IEnumerable<int> GetAdditiveModifiers(Stat stat)
        {   if (stat == Stat.Damage) {
                yield return currentWeapon.GetDamage();
            }
        }

        void Hit() // Animation Event
        {
            if (target == null) return;

            int damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile()) {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            } 
            else {
                target.TakeDamage(gameObject, damage); //currentWeapon.GetDamage());   // Deal damage on attack hit
            }
        }

        void Shoot() // Animation Event
        {
            Hit();
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(currentWeapon.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            string weaponName = state.ToObject<string>();
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}
