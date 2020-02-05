using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    // kolajder za napad za vuka
    public Wolf wolf;

    void OnTriggerEnter(Collider col){
        if (col.gameObject.tag == "witch"){
            wolf.dealDmg = true;
        }
    }

    void OnTriggerExit(Collider col){
        if (col.gameObject.tag == "witch"){
            wolf.dealDmg = false;
            wolf.agent.ResetPath(); // resetp path nakon sto pobegne player
        }
    }
}
