using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Attributes;
using RPG.Core;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, IJsonSaveable
    {
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxPathLength = 30f;

        NavMeshAgent navmeshAgent;
        Animator animator;
        Health health;

        private void Awake()
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

        public bool CanMoveTo(Vector3 destination) {
            // Check if path exists and is complete
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath || path.status != NavMeshPathStatus.PathComplete) return false;

            // Check that path is less than maxPathLength
            if (GetPathLength(path) > maxPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float distance = 0f;
            if (path.corners.Length < 2) return distance;

            for (int i = 1; i < path.corners.Length; i++) {
                distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
            
            return distance;
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

        public JToken CaptureAsJToken()
        {
            return transform.position.ToToken();
        }

        public void RestoreFromJToken(JToken state)
        {
            navmeshAgent.enabled = false;
            transform.position = state.ToVector3();
            navmeshAgent.enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}


