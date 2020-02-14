using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public Wolf wolf;
    private float spawnTimer;
    private Vector3 spawnPoint;
    public GameObject points;
    public int wolfCount;
    public int wolfsKilled;
    public Text scoreLabel;

    void Start(){
        spawnTimer = 0f;
        wolfsKilled = 0;
    }

    void Update(){
        if (wolfCount < 4){
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f){
                foreach (GameObject wolf in GameObject.FindGameObjectsWithTag("wolf")){
                    if (wolf.GetComponent<Wolf>().enabled == false){
                        Destroy(wolf);
                    }
                }
                int pointsChild = Random.Range(0, 5);
                spawnPoint = points.transform.GetChild(pointsChild).transform.position;
                Wolf wolfClone = Instantiate(wolf, spawnPoint, Quaternion.identity);
                spawnTimer = Random.Range(4f, 8f);
                wolfCount += 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene("SampleScene");
        }

        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }

        scoreLabel.text = "Wolfs killed: " + wolfsKilled;
    }
}
