using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navmeshAgent;
        Animator animator;
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
            navmeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }
        void Update()
        {
            navmeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        private void UpdateAnimator()   // set speed of animation to match player velocity
        {
            Vector3 velocity = transform.InverseTransformDirection(navmeshAgent.velocity);  // change from global to local space
            animator.SetFloat("forwardSpeed", velocity.z);  // z is forward
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)  // Cancel attack action and start MoveTo action
        {
            
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)   // move to destination and restart navmeshAgent if stopped
        {
            navmeshAgent.destination = destination;
            navmeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);  // force speedFraction between 0 - 1
            navmeshAgent.isStopped = false;
        }

        public void Cancel()  // stop navmeshAgent
        {
            navmeshAgent.isStopped = true;
        }
    }
}


