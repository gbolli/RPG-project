using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
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

        private void UpdateAnimator()   // set speed of animation to match player velocity
        {
            Vector3 velocity = transform.InverseTransformDirection(navmeshAgent.velocity);  // change from global to local space
            animator.SetFloat("forwardSpeed", velocity.z);  // z is forward
        }

        public void StartMoveAction(Vector3 destination)  // Cancel attack action and start MoveTo action
        {
            GetComponent<ActionScheduler>().StartAction(this);
            GetComponent<Fighter>().Cancel();
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)   // move to destination and restart navmeshAgent if stopped
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().destination = destination;
            navmeshAgent.isStopped = false;
        }

        public void Stop()  // stop navmeshAgent
        {
            navmeshAgent.isStopped = true;
        }
    }
}


