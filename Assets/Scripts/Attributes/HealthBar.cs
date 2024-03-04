using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes {
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;

        void Update()
        {
            float xValue = healthComponent.GetPercentageHealth();
            foreground.localScale = new Vector3(xValue,1,1);
        }
    }
}

