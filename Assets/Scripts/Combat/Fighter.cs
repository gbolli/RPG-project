using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
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
        [SerializeField] WeaponConfig defaultWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;
        Health target;

        // LazyValue is custom initializer package from GameDevTV to ensure initialization before use
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        private void Awake() {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private void Start() {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null || target.IsDead()) return;   // if not attacking or target dead then return

            if (Vector3.Distance(transform.position, target.transform.position) <= currentWeaponConfig.GetRange())
            {
                GetComponent<Mover>().Cancel(); // if within attacking distance, stop
                AttackBehavior();
            }
            else GetComponent<Mover>().MoveTo(target.transform.position, 1f);  // if not within attacking distance, keep moving
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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
            return target != null 
                && !target.GetComponent<Health>().IsDead() 
                && GetComponent<Mover>().CanMoveTo(target.transform.position);
        }

        public IEnumerable<int> GetAdditiveModifiers(Stat stat)
        {   if (stat == Stat.Damage) {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<int> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage) {
                yield return currentWeaponConfig.GetPercentageBonus();
            }
        }

        void Hit() // Animation Event
        {
            if (target == null) return;

            if (currentWeapon.value != null) currentWeapon.value.OnHit();

            int damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeaponConfig.HasProjectile()) {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
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
            return JToken.FromObject(currentWeaponConfig.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            string weaponName = state.ToObject<string>();
            WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weapon);
        }

    }
}
