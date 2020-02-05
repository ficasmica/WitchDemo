using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GestureRecognizer;
using UnityEngine.UI;

public class Witch : MonoBehaviour
{
    private Animator anim;
    public GameObject wolf;
    public Fireball fireball;
    public Meteor meteor;
    public DrawDetector detector;
    public static bool isCasting = false;
    private string spell;
    public static Vector3 target;
    public static float health;
    public float maxHealth;
    private bool receiveDmg;
    public float dmgRate = 1f;
    private Image healthBar;
    public GameObject healthBarObj;
    private Vector3 movement;
    private float speed;
    public float initSpeed;
    private Rigidbody rb;
    private bool isShooting = false;
    private float castTime = 0f;
    public float initCastTime;
    private LineRenderer lr;
    public Transform castPoint;
    public Transform rayPoint;

    void Start(){
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        anim.SetBool("isRunning", false);
        anim.SetBool("isRay", false);
        healthBar = healthBarObj.GetComponent<Image>();
        health = maxHealth;
    }

    void Update(){
        if (!isShooting){
            speed = initSpeed;
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;
            if (movement != Vector3.zero){
                anim.SetBool("isRunning", true);
            }
            else{
                anim.SetBool("isRunning", false);
            }
        }
        else{
            speed = 0f;
        }
        if (castTime > 0f){
            castTime -= Time.deltaTime;
            if (castTime <= 0f){
                isShooting = false;
            }
        }

        // Kastanje
        if (isCasting){
            switch(spell){
                case "Boulder":
                    if (Input.GetMouseButtonDown(0)){
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                            anim.SetTrigger("summon");
                            isShooting = true;
                            Meteor cloneMeteor = Instantiate(meteor, new Vector3(hit.point.x, 60f, hit.point.z), Quaternion.identity) as Meteor;
                            castTime = initCastTime;
                        }
                    }
                    if (Input.GetMouseButtonUp(0)){
                        isCasting = false;
                        detector.GetComponent<DrawDetector>().enabled = true;
                        spell = null;
                    }
                    break;
            
                case "Fireball":
                    if (Input.GetMouseButtonDown(0)){
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                            target = hit.point;
                            anim.SetTrigger("castBolt");
                            isShooting = true;
                            transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
                            Fireball cloneFireball = Instantiate(fireball, castPoint.position, transform.rotation) as Fireball; 
                            castTime = initCastTime;
                        }
                    }
                    if (Input.GetMouseButtonUp(0)){
                        isCasting = false;
                        detector.GetComponent<DrawDetector>().enabled = true;
                        spell = null;
                    }
                    break;
            
                case "Blink":
                    if (Input.GetMouseButtonDown(0)){
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                            //transform.position = hit.point;
                            rb.MovePosition(hit.point);
                        }
                    }
                    if (Input.GetMouseButtonUp(0)){
                        isCasting = false;
                        detector.GetComponent<DrawDetector>().enabled = true;
                        spell = null;
                    }
                    break;
            
                case "Ray":
                    if (Input.GetMouseButton(0)){
                        isShooting = true;
                        anim.SetBool("isRay", true);
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
                            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                            lr.enabled = true;
                            lr.SetPosition(0, rayPoint.position);
                        }
                        RaycastHit rayhit;
                        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out rayhit, Mathf.Infinity)){
                            if (rayhit.collider.gameObject.tag == "wolf"){
                                Destroy(rayhit.collider.gameObject);
                            }
                        lr.SetPosition(1, rayhit.point);
                        }
                    }
                    if (Input.GetMouseButtonUp(0)){
                        anim.SetBool("isRay", false);
                        lr.enabled = false;
                        isCasting = false;
                        isShooting = false;
                        detector.GetComponent<DrawDetector>().enabled = true;
                        spell = null;
                    }
                    break;
            
                case "Heal":
                    anim.SetTrigger("summon");
                    isShooting = true;
                    health += 0.3f;
                    if (health >= 1){
                        health = 1f;
                    }
                    isCasting = false;
                    spell = null;
                    castTime = initCastTime;
                    detector.GetComponent<DrawDetector>().enabled = true;
                    break;
            }
        }

        // Health kontrola
        healthBar.fillAmount = health / maxHealth;
        Debug.Log(healthBar.fillAmount);
        if (health <= 0f){
            anim.SetTrigger("dead");
            GetComponent<Witch>().enabled = false;
            //Destroy(gameObject);
        }
    }

    void FixedUpdate(){
        rb.velocity = movement * speed;
        if (movement != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }
    }

    public void GetSpell(RecognitionResult result){
        if (result != RecognitionResult.Empty){
            isCasting = true;
            spell = result.gesture.id;
            detector.GetComponent<DrawDetector>().enabled = false;
            detector.ClearLines();
        }
        else{
            isCasting = false;
            detector.ClearLines();
        }
    }
}
