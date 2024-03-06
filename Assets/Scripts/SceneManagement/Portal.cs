using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.Control;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier {
            A1, A2, B1, B2, C1, C2, C3
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] Transform spawnPoint;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.Log("SceneToLoad is not set");
                yield break;   // yield break needed to escape from IEnumerator (instead of return)
            }

            DontDestroyOnLoad(gameObject);  // prevent destroy when new scene is loaded
            // Portals need to be at scene root level for DontDestroyOnLoad

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();

            // Remove last scene player controls to avoid multiple transitions if re-entering portal immediately
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            // Save current level before portal
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);  // LoadSceneAsync return object that unity will use

            // Remove new scene player controls to avoid multiple transitions if re-entering portal immediately (after new scene loads)
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            // Load stats (player health, etc) before new location
            wrapper.Load();

            Portal otherPortal = GetTheOtherPortal();
            UpdatePlayer(otherPortal);

            // Save position in new scene
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            // Restore player controls
            newPlayerController.enabled = true;

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
                if (portal.destination != destination || portal == this) continue;  // we don't want this portal
                return portal;
            }
            return null;  // can't find portal
        }
    }
}

