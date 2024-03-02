using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utilites {
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] GameObject objectToDestroy;
        
        public void DestroyText() {
                Destroy(objectToDestroy);
        }
    }
}
