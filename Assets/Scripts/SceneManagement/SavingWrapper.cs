using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 0.5f;

        IEnumerator Start() {
            Fader fader = FindObjectOfType<Fader>();

            fader.FadeOutImmediate();

            yield return GetComponent<JsonSavingSystem>().LoadLastScene(defaultSaveFile);

            yield return fader.FadeIn(fadeInTime);
        }
        const string defaultSaveFile = "save";

        private void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
            // KeyCode.Backspace needed for MacOS delete key.  Will be in menu in the future.
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<JsonSavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<JsonSavingSystem>().Save(defaultSaveFile);
        }

        public void Delete() {
            GetComponent<JsonSavingSystem>().Delete(defaultSaveFile);
        }
    }
}
