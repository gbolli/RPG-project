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

        private void Awake() {
            StartCoroutine(LoadLastScene());
        }

        // Rename from Start -> LoadLastScene and call in Awake to prevent race condition issues with other Start methods needing loaded information 
        IEnumerator LoadLastScene() {
            yield return GetComponent<JsonSavingSystem>().LoadLastScene(defaultSaveFile);
            // fader after yield return to avoid race condition
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
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
