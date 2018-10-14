using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_MOV : MonoBehaviour {



    /*
     * 
     *  SOUND BASED MOVEMENT: 
     *      The sphere collider of this object will grow depending on the speed of the player.
     *      There is an absolute max of the collider's radius based upon whether the player is walking, running, or sneaking. 
     * 
     */ 

    Vector3 targetDestination;
    Vector3 playerPosition;
    Vector3 destination;
    [SerializeField] Rigidbody rb;
    public Collider[] hitColliders;

    [SerializeField] bool clicked;
    [SerializeField] float maxSpeed;
    [SerializeField] bool moving;

    float defaultSpeed = 12f;
    float colMod = .5f;
    float maxCol;
    float walkSpeed;
    float stopRadius;
    float waitTime = 0f;
    float maxTime = .5f;
    float radius = .5f;


    void Start()
    {
        walkSpeed = 6f;
        stopRadius = 2f;
        maxSpeed = defaultSpeed;
    }

    void Update(){

        
        MouseClicked();
        RunOrWalk();
        if (clicked){

            SetDestination();
            GoToDestination();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // DETERMINE SPEEDS AND COLLIDER ABSOLUTES WHEN WALKING OR RUNNING
    private void RunOrWalk(){
        int enemyLayer = 1 << LayerMask.NameToLayer("enemy");
        if (moving){
            if (Input.GetKey(KeyCode.LeftShift)){
                maxCol = 2.5f;
                maxSpeed = walkSpeed;
                waitTime += Time.deltaTime * .25f;

                if (radius > maxCol)
                    radius = .5f;
                else if (waitTime <= maxTime){
                    if (radius != maxCol)
                        radius += colMod;
                    hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
                    waitTime = 0;
                }
            }
            else{
                maxCol = 5.5f;
                maxSpeed = defaultSpeed;
                waitTime += Time.deltaTime * .5f;

                 if (radius > maxCol)
                    radius = .5f;
                else if (waitTime <= maxTime){
                    if (radius != maxCol)
                        radius += colMod;
                    hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
                    waitTime = 0;
                }
            }
        }
        else{
            waitTime += Time.deltaTime * .5f;
            if (waitTime <= maxTime){
                if (radius != .5f)
                    radius -= colMod;
                hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
                waitTime = 0;
            }
        }

        // SEE WHICH ENEMY IS DETECTING PLAYER SOUND
        foreach (Collider enemy in hitColliders)
             Debug.Log(enemy.gameObject.name);
    }

    // CHECK IF PLAYER IS CLICKING
    private void MouseClicked(){
        // GET MOUSE CLICK POSITION WHEN CLICKED, IF CLICKED ON THE FLOOR
        RaycastHit hit;
        if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
            clicked = true;
            if (hit.collider.tag == "Floor")
                targetDestination = hit.point;
        }
    }

    // MOVE PLAYER TO CLICK LOCATION
    private void SetDestination(){
        // CREATE PATH TO DESTINATION
        destination = targetDestination - transform.position;
        transform.LookAt(targetDestination);
    }

    // CHECK IF ARRIVED TO DESTINATION
    private void GoToDestination(){
        // MOVE
        if (destination.magnitude > stopRadius){
            moving = true;
            destination.Normalize();
            destination *= maxSpeed;
            rb.velocity = destination;
        }
        // STOP
        else{
            moving = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            clicked = false;
        }
    }
}
