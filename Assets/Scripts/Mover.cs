using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        GetComponent<UnityEngine.AI.NavMeshAgent>().destination = target.position;
    }
}
