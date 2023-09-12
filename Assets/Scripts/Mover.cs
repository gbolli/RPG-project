using System;
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
            MoveToCursor();
        }

        Debug.DrawRay(lastRay.origin, lastRay.direction * 100, Color.white);

        // GetComponent<UnityEngine.AI.NavMeshAgent>().destination = target.position;
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.point;
        }
    }
}
