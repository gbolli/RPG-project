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

        GameObject player;
        Fighter fighter;
        Mover mover;
        Health health;

        // enemy state
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

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
            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))  // Attack state
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player);
            }
            else if (timeSinceLastSawPlayer < suspicionTime)  // Suspicious state
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();  // Cancel from action scheduler
            }
            else
            {
                mover.StartMoveAction(guardPosition);  // Return to guard state
            }
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

