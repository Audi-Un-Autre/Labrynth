using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [SerializeField] private float moveSpeed;
    [SerializeField] private float runMultiplier;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackDistance;
    [SerializeField] private GameObject[] enemies;
    private Transform target;
    private float distance;
    public static float maxHealth;
    public static float currHealth;
    public float trackHealth;

    private CharacterController character;
    private Vector3 movement;
    static Animator anim;
    public Collider[] hitColliders;
    [SerializeField] bool moving;
    public Animation anima;

    float defaultSpeed = 12f;
    float colMod = .5f;
    float maxCol;
    float walkSpeed;
    float stopRadius;
    float waitTime = 0f;
    float maxTime = .5f;
    float radius = .5f;
    [SerializeField] float maxSpeed;
    Rigidbody rb;
    Vector3 velocity;

    ENEMY_AI thisEnemy;
    int enemyLayer;

    // Use this for initialization
    void Start ()
    {
        maxHealth = 20;
        currHealth = 10;
        enemyLayer = 1 << LayerMask.NameToLayer("enemy");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        walkSpeed = 6f;
        stopRadius = 2f;
        maxSpeed = defaultSpeed;
        character = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        trackHealth = currHealth;
        RunOrWalk();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            moving = true;
        else
            moving = false;

        Cursor.visible = false;
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
            anim.SetBool("isRunning", false);
        }

        //Move back
        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("runBack", true);
            movement = (transform.forward * Input.GetAxis("Vertical"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
        }
        else
        {
            anim.SetBool("runBack", false);
        }

        //Move left
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W)))
        {
            anim.SetBool("runLeft", true);
            movement = (transform.right * Input.GetAxis("Horizontal"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
        }
        else
        {
            anim.SetBool("runLeft", false);
        }

        //Move right
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)))
        {
            anim.SetBool("runRight", true);
            movement = (transform.right * Input.GetAxis("Horizontal"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
        }

        else
        {
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
        if (moving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                maxCol = 5f;
                waitTime += Time.deltaTime * .25f;

                if (radius > maxCol)
                    radius = .5f;
                else if (waitTime <= maxTime)
                {
                    if (radius != maxCol)
                        radius += colMod;
                    //hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
                    waitTime = 0;
                }
            }
            else
            {
                maxCol = 2f;
                waitTime += Time.deltaTime * .5f;

                if (radius > maxCol)
                    radius = .5f;
                else if (waitTime <= maxTime)
                {
                    if (radius != maxCol)
                        radius += colMod;
                    //hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
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
                //hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);
                waitTime = 0;
            }
        }
        hitColliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        // SEE WHICH ENEMY IS DETECTING PLAYER SOUND
        for (var i = 0; i < hitColliders.Length; i++){
            Debug.Log(hitColliders[i].gameObject.name);
            thisEnemy = hitColliders[i].GetComponent<ENEMY_AI>();
            thisEnemy.playerHeard = true;
            thisEnemy.state = ENEMY_AI.GameState.ALERTED;
            thisEnemy.heardAt = this.transform.position;
        }
    }

    private static void takeDamage()
    {
        anim.SetBool("isHit", true);
    }

    public static void HealPlayer(int healAmount)
    {
        currHealth += healAmount;
        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

    }

    public static void HurtPlayer(int damage)
    {
        currHealth -= damage;
        print("Current health is: " + currHealth);
        //takeDamage();
        //anim.SetBool("isHit", false);
    }

    public void EnemyHit()
    {
        GameObject closestEnemy = enemies[0];
        float dist = Vector3.Distance(transform.position, enemies[0].transform.position);
        for (int i = 0; i < enemies.Length; i++)
        {
            var tempDistance = Vector3.Distance(transform.position, enemies[i].transform.position);
            if (tempDistance < dist)
            {
                closestEnemy = enemies[i];
            }
        }
        var enDis = Vector3.Distance(transform.position, closestEnemy.transform.position);
        print(closestEnemy);
        if (enDis <= attackDistance)
        {
            EnemyController.HurtEnemy(damage, closestEnemy);
        }
    }

}

// 2. Sword push enemies
// 3. When character heavy attacks neeeds to stay in position on finish
// 4. Set animation using trigger
// 6. Stop moving when attacking