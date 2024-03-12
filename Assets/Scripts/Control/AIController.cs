using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        [SerializeField] float agroCooldownTime = 5f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float waypointTolerance = 1f;

        [SerializeField] PatrolPath patrolPath;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        
        GameObject player;
        Fighter fighter;
        Mover mover;
        Health health;

        // enemy state
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;

        int currentWaypointIndex = 0;

        private void Awake() {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Start()
        {
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            UpdateTimers();

            // Check if player in range
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void AttackBehavior()  // Attack state
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void SuspicionBehavior()  // Suspicious state
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();  // Cancel from action scheduler
        }

        private void PatrolBehavior() // Patrol state
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        public void Aggrevate() {
            timeSinceAggrevated = 0;
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < chaseDistance) Aggrevate();
            
            return timeSinceAggrevated < agroCooldownTime;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

