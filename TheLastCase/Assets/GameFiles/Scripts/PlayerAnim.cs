using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnim : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    private float Speed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Speed = agent.velocity.magnitude;

        anim.SetFloat("Speed", Speed);
    }
}
