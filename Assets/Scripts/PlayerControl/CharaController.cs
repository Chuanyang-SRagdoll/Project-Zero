using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharaController : MonoBehaviour
{
    [SerializeField] float speed;
    Vector3 targetPos;
    NavMeshAgent agent;
    bool isMoving;

    private void Start ( )
    {
        agent = GetComponent<NavMeshAgent> ( );

    }


    // Movement method
    public void  MoveTo( Vector3 taPos)
    {
        targetPos = taPos;
        agent.speed = speed;
        agent.SetDestination ( targetPos );
        transform.LookAt ( targetPos );
    }



}





