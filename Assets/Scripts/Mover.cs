using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;

    Ray lastRay;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("mouse button 0 down");
        }

        Debug.DrawRay(lastRay.origin, lastRay.direction * 100, Color.white);

        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = target.position;
    }
}
