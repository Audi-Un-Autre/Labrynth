using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    [SerializeField] private float runMultiplier;
    private CharacterController character;
    private Vector3 movement;
    static Animator anim;
    public Collider[] hitColliders;
    [SerializeField] bool moving;

    float defaultSpeed = 12f;
    float colMod = .5f;
    float maxCol;
    float walkSpeed;
    float stopRadius;
    float waitTime = 0f;
    float maxTime = .5f;
    float radius = .5f;
    [SerializeField] float maxSpeed;



    // Use this for initialization
    void Start ()
    {
        walkSpeed = 6f;
        stopRadius = 2f;
        maxSpeed = defaultSpeed;
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        RunOrWalk();
    }

    // Update is called once per frame
    // *** UPDATE CHANGED FROM LATEUPDATE to FIXEDUPDATE to sync with CAMERACONTROLLER
    void FixedUpdate ()
    {

        //Basic attack
        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetBool("isAttacking", true);
            /**/moveSpeed = 0.0f;
        }
        else
        {
            anim.SetBool("isAttacking", false);
            /**/moveSpeed = 6.0f;
        }

        //Heavy attack
        if (Input.GetKey(KeyCode.Mouse1))
        {
            anim.SetBool("HeavyAttack", true);
            /**/moveSpeed = 0.0f;
        }
        else
        {
            anim.SetBool("HeavyAttack", false);
            /**/moveSpeed = 6.0f;
        }

        //Move forward
        if (Input.GetKey(KeyCode.W))
        {
            moving = true;
            anim.SetBool("isRunning", true);
            movement = (transform.forward * Input.GetAxis("Vertical"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
            if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                anim.SetBool("isRunning", true);
                movement = (transform.forward * Input.GetAxis("Vertical"));
                movement = movement.normalized * (moveSpeed * runMultiplier);
                character.Move(movement * Time.deltaTime);
            }
        }
        else
        {
            moving = false;
            anim.SetBool("isRunning", false);
        }

        //Move back
        if (Input.GetKey(KeyCode.S))
        {
            moving = true;
            anim.SetBool("runBack", true);
            movement = (transform.forward * Input.GetAxis("Vertical"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
        }
        else
        {
            moving = false;
            anim.SetBool("runBack", false);
        }

        //Move left
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W)))
        {
            moving = true;
            anim.SetBool("runLeft", true);
            movement = (transform.right * Input.GetAxis("Horizontal"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
        }
        else
        {
            moving = false;
            anim.SetBool("runLeft", false);
        }

        //Move right
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)))
        {
            moving = true;
            anim.SetBool("runRight", true);
            movement = (transform.right * Input.GetAxis("Horizontal"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
        }

        else
        {
            moving = false;
            anim.SetBool("runRight", false);
        }

        // rotate cam right and left
        float mouseInput = Input.GetAxis("Mouse X") * moveSpeed;
        Vector3 rotateCam = new Vector3(0, mouseInput, 0);
        transform.Rotate(rotateCam);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // DETERMINE SPEEDS AND COLLIDER ABSOLUTES WHEN WALKING OR RUNNING
    private void RunOrWalk()
    {
        int enemyLayer = 1 << LayerMask.NameToLayer("enemy");
        if (moving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                maxCol = 2.5f;
                waitTime += Time.deltaTime * .25f;

                if (radius > maxCol)
                    radius = .5f;
                else if (waitTime <= maxTime)
                {
                    if (radius != maxCol)
                        radius += colMod;
                    hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
                    waitTime = 0;
                }
            }
            else
            {
                maxCol = 5.5f;
                waitTime += Time.deltaTime * .5f;

                if (radius > maxCol)
                    radius = .5f;
                else if (waitTime <= maxTime)
                {
                    if (radius != maxCol)
                        radius += colMod;
                    hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
                    waitTime = 0;
                }
            }
        }
        else
        {
            waitTime += Time.deltaTime * .5f;
            if (waitTime <= maxTime)
            {
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
}

// 2. Sword push enemies
// 3. When character heavy attacks neeeds to stay in position on finish
// 4. Set animation using trigger
// 6. Stop moving when attacking