using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPrefab;   // designed for Fader, persist through scene changes without singleton

        static bool hasSpawned = false;  // carries over life of application (not class instance)

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistantObjects();

            hasSpawned = true;
        }

        private void SpawnPersistantObjects()
        {
            GameObject persistantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}

