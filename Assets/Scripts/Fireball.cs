using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    private float lifeTime = 4f;

    void Start(){
        transform.LookAt(new Vector3(Witch.target.x, transform.position.y, Witch.target.z)); // rotiraj ka klik target
    }

    void Update(){
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f){
            Destroy(gameObject);
        }
    }

    void FixedUpdate(){
        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag != "witch"){
            Destroy(gameObject);
        }
    }
}
