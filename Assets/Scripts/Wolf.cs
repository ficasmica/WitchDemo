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
    public EnemySpawner spawner;
    public bool isDead = false;
    public bool isSlowed = false;
    public float slowedTime;
    private float delTime = 8f;
    public bool isRayed = false;

    void Start(){
        spawner = GameObject.FindGameObjectWithTag("spawner").GetComponent<EnemySpawner>();
        agent = GetComponent<NavMeshAgent>();
        healthBar = healthBarObj.GetComponent<Image>();
        anim = GetComponent<Animator>();
        agent.SetDestination(target);
        anim.SetBool("isRunning", true);
        pathTimer = newPathTime;
        health = maxHealth;
        slowedTime = 3f;
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
                Witch.health -= 0.25f;
                dealDmgRate = 1.9f;
            }
        }
        else{
            agent.isStopped = false;
            anim.SetBool("isRunning", true);
            anim.SetBool("isAttacking", false);
        }

        if (health <= 0){
            isDead = true;
            agent.isStopped = true;
            anim.speed = 1f;
            anim.SetTrigger("dead");
            spawner.wolfCount -= 1;
            spawner.wolfsKilled += 1;
            GetComponent<Wolf>().enabled = false;
        }

        if (isDead){
            GetComponent<BoxCollider>().enabled = false;
            delTime -= Time.deltaTime;
            if (delTime <= 0f){
                Destroy(gameObject);
            }
        }

        if (isSlowed){
            slowedTime -= Time.deltaTime;
            if (slowedTime <= 0f){
                agent.speed *= 2f;
                anim.speed *= 2f;
                slowedTime = 3f;
                isSlowed = false;
                isRayed = false;
            }
        }

        if (!isRayed){
            agent.speed = 12f;
            anim.speed = 1f;
        }

        healthBar.fillAmount = health / maxHealth;
    }

    void OnTriggerEnter(Collider col){
        switch(col.gameObject.tag){
            case "fireball":
                health -= 0.5f;
                isRayed = true;
                agent.speed *= 0.5f;
                anim.speed *= 0.5f;
                isSlowed = true;
                healthBar.fillAmount = health / maxHealth;
                break;
        }
    }
}
