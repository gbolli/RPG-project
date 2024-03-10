using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] int healthToRestore = 0;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null) subject.GetComponent<Fighter>().EquipWeapon(weapon);
            subject.GetComponent<Health>().Heal(healthToRestore);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float respawnTime)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(respawnTime);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;

            // loop through children
            foreach (Transform child in transform) {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0)) {
                // TODO: Need to move to pickup first
                // callingController.GetComponent<Mover>().MoveTo(this.transform.position, 20f);
                Pickup(callingController.gameObject);
            }
            
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.WeaponPickup;
        }
    }
}
