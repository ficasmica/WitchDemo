using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{   
    public float speed;

    void Update(){
        
    }

    void FixedUpdate(){
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "floor"){
            Destroy(gameObject);
        }
    }
}
