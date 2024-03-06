using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace RPG.Core {
    public class CameraFacing : MonoBehaviour
    {     
        void LateUpdate()
        {
            // which is better?   transform.rotation or transform.forward
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
