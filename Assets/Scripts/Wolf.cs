using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{
    public NavMeshAgent agent;
    private Vector3 target;
    private float pathTimer;
    public float newPathTime;
    private Animator anim;
    public float maxHealth;
    public float health;
    public GameObject healthBarObj;
    private Image healthBar;
    public bool dealDmg = false;
    public float dealDmgRate = 0f;
    public float attackWaitTime;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        healthBar = healthBarObj.GetComponent<Image>();
        anim = GetComponent<Animator>();
        agent.SetDestination(target);
        anim.SetBool("isRunning", true);
        pathTimer = newPathTime;
        health = maxHealth;
    }

    void Update(){
        pathTimer -= Time.deltaTime;
        if (pathTimer <= 0f){
            target = GameObject.FindGameObjectWithTag("witch").transform.position;
            agent.SetDestination(target);
            pathTimer = newPathTime;
        }

        if (!agent.pathPending){
            if (agent.remainingDistance <= agent.stoppingDistance){
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f){
                    target = GameObject.FindGameObjectWithTag("witch").transform.position;
                    agent.SetDestination(target);
                }
            }
        }

        if (dealDmg){
            agent.isStopped = true;
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", true);
            dealDmgRate -= Time.deltaTime;
            if (dealDmgRate <= 0f){
                Witch.health -= 0.1f;
                dealDmgRate = 1.9f;
            }
        }
        else{
            agent.isStopped = false;
            anim.SetBool("isRunning", true);
            anim.SetBool("isAttacking", false);
        }

        if (health == 0){
            agent.isStopped = true;
            anim.SetTrigger("dead");
        }
    }

    void OnTriggerEnter(Collider col){
        switch(col.gameObject.tag){
            case "fireball":
                health -= 0.5f;
                healthBar.fillAmount = health / maxHealth;
                break;
        }
    }
}
