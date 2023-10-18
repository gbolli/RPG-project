using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad;

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
            print("Scene Loaded");
            Destroy(gameObject);  // destroy to clean up after code is run
        }
    }
}

