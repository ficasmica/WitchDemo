using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{   
    void Start(){
        foreach (Transform child in transform){
            child.localScale *= Random.Range(0.8f, 1.2f);
        }
    }
}
