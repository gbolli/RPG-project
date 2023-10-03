using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        float suspicionTime = 5f;

        [SerializeField] PatrolPath patrolPath;
        float waypointTolerance = 1f;

        GameObject player;
        Fighter fighter;
        Mover mover;
        Health health;

        // enemy state
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            timeSinceLastSawPlayer += Time.deltaTime;

            if (health.IsDead()) return;

            // Check if player in range
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))  
            {
                timeSinceLastSawPlayer = 0;
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

        private void AttackBehavior()  // Attack state
        {
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
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition);
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

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

