using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wolf wolf;
    private float spawnTimer;
    private Vector3 spawnPoint;
    public GameObject points;
    public int wolfCount;

    void Start(){
        spawnTimer = 0f;
    }

    void Update(){
        if (wolfCount < 4){
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f){
                int pointsChild = Random.Range(0, 5);
                spawnPoint = points.transform.GetChild(pointsChild).transform.position;
                Wolf wolfClone = Instantiate(wolf, spawnPoint, Quaternion.identity);
                spawnTimer = Random.Range(4f, 8f);
                wolfCount += 1;
            }
        }
    }
}
