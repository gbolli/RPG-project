using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
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
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = transform.InverseTransformDirection(navmeshAgent.velocity);  // change from global to local space
            animator.SetFloat("forwardSpeed", velocity.z);  // z is forward
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = destination;
            navmeshAgent.isStopped = false;
        }

        public void Stop()
        {
            navmeshAgent.isStopped = true;
        }
    }
}


