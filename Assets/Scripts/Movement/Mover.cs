using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    NavMeshAgent navmeshAgent;
    Animator animator;

    private void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = transform.InverseTransformDirection(navmeshAgent.velocity);  // change from global to local space
        animator.SetFloat("forwardSpeed", velocity.z);  // z is forward
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            MoveTo(hit.point);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = destination;
    }
}
