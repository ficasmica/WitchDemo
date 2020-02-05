using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Troll : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 target;
    private float pathTimer;
    public float newPathTime;
    private Animator anim;
    public float maxHealth;
    public float health;
    public GameObject healthBarObj;
    private Image healthBar;

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

        if (health == 0){
            agent.isStopped = true;
            anim.SetTrigger("isDead");
        }
    }

    void OnTriggerEnter(Collider col){
        switch(col.gameObject.tag){
            case "fireball":
                health -= 0.5f;
                //healthBar.fillAmount = health / maxHealth;
                break;
        }
    }
}
