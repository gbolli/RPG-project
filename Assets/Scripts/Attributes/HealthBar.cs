using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        void Update()
        {
            float xValue = healthComponent.GetPercentageHealth();

            // Visibility of Healthbar (hidden at full health and dead)
            if (xValue == 1 || xValue == 0) {
                rootCanvas.enabled = false;
                return;
            }
            
            foreground.localScale = new Vector3(xValue,1,1);
            rootCanvas.enabled = true;
        }
    }
}

