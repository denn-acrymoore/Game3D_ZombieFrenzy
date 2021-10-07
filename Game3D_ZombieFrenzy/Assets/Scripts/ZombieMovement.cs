using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position); //set si zombie lari ke arah target
        anim.SetBool("IsRunning", true);

        float distance = Vector3.Distance(transform.position, target.position);

        if(distance < 2)
        {
            anim.SetBool("Attacking", true);
        }
    }

    public void enemyDeathStop()
    {
        agent.updatePosition = false;
    }
}
