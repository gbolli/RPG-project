using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad;
        [SerializeField] Transform spawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(gameObject);  // prevent destroy when new scene is loaded
            // Portals need to be at scene root level for DontDestroyOnLoad
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);  // LoadSceneAsync return object that unity will use

            Portal otherPortal = GetTheOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);  // destroy to clean up after code is run
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.transform.position);
            // player.transform.position = otherPortal.spawnPoint.transform.position;
            player.transform.rotation = otherPortal.spawnPoint.transform.rotation;
        }

        private Portal GetTheOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;  // we don't want this portal
                return portal;
            }
            return null;  // can't find portal
        }
    }
}

