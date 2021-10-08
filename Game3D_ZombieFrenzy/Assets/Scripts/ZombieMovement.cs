using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    Animator anim;

    bool isAttacking = false;
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position); //set si zombie lari ke arah target
        anim.SetBool("IsRunning", true);

        float distance = Vector3.Distance(transform.position, target.position);

        if(distance < 2 && !isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attacking");

            AttackPlayer();
        }
    }

    public void SetZombieDeath()
    {
        isAlive = false;
    }

    void AttackPlayer()
    {
        if (GameManagerScript.isPlayerAlive && isAlive)
        {
            Debug.Log("Attacked the player");
            IDamageable damageable = target.gameObject.GetComponent<PlayerHealth>();
            damageable.TakeDamage(1, null);
        }
    }

    void EnemyStopAttackingAnim()
    {
        //Debug.Log("Enemy has stop Attacking");

        isAttacking = false;
    }

    public void enemyDeathStop()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }
}
