using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        IEnumerator Start() {
            yield return GetComponent<JsonSavingSystem>().LoadLastScene(defaultSaveFile);
        }
        const string defaultSaveFile = "save";

        private void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
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
    }
}
