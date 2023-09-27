using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        GameObject player;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            // Check if player in range
            if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
            {
                print(gameObject.name + " is chasing you!");
            }
        }
    }
}

