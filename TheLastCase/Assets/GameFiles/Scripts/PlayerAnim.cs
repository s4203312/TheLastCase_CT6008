using UnityEngine;
using UnityEngine.AI;

public class PlayerAnim : MonoBehaviour         //Player Animation
{
    private NavMeshAgent agent;
    private Animator anim;

    private float Speed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Speed = agent.velocity.magnitude;

        anim.SetFloat("Speed", Speed);
    }
}
