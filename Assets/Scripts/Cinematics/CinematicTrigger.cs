using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        bool hasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player") && !hasPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                hasPlayed = true;
            }
            
        }
    }
}

