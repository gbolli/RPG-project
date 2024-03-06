using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utilites {
    public class AudioClipRandomizer : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClipArray = null;
        [SerializeField] AudioSource audioSource = null;

        public void PlayRandomAudioClip() {
            if (audioSource != null) audioSource.PlayOneShot(audioClipArray[Random.Range(0, audioClipArray.Length)]);
        }
    }
}
