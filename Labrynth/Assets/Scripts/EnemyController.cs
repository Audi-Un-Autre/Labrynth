using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform enemy;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private int attackDistance;
    [SerializeField] Rigidbody rb;
    [SerializeField] private int damage = 1;
    [SerializeField] public static float maxHealth;
    [SerializeField] public static float currHealth;
    static Animator anim;

    ENEMY_AI ai;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        maxHealth = 5;
        currHealth = maxHealth;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Vector3.Distance(target.position, this.transform.position) < detectionDistance) // MINIMUM DISTANCE TO START FOLLOW
        {
            Vector3 direction = target.position - enemy.position;
            if (direction.magnitude < attackDistance) // RADIUS OF SATISFACTION
            {
                anim.SetBool("isAttacking", true);
                anim.SetBool("isMoving", false);
                //enemy.Translate(0.0f, 0.0f, 0.0f);
                // STOP MOVEMENT
                //rb.velocity = Vector3.zero;
                //rb.angularVelocity = Vector3.zero;

            }
            else if (direction.magnitude > attackDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                anim.SetBool("isMoving", true);
                anim.SetBool("isAttacking", false);
                // START MOVEMENT
                /*
                 * THIS IS HANDLED IN THE ENEMY AI
                direction.Normalize();
                direction *= moveSpeed;
                rb.velocity = direction;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                anim.SetBool("isMoving", true);
                */
            }
        }
        else
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", false);
            // STOP MOVEMENT
            /* 
             * THIS IS HANDLED IN THE ENEMY AI
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            */
        }

        if (currHealth <= 0)
        {
            anim.SetBool("isDead", true);
            moveSpeed = 0.0f;
        }
    }
    /*
    public void RegisterHit()
    {
        if (Vector3.Distance(target.position, this.transform.position) < detectionDistance)
        {
            Vector3 direction = target.position - enemy.position;
            if (direction.magnitude < attackDistance)
            {
                PlayerController.HurtPlayer(damage);
            }
        }
    }
    */
    public static void HurtEnemy(int damage, GameObject character)
    {
        currHealth -= damage;
        print(character + " health is: " + currHealth);
        //takeDamage();
        //anim.SetBool("isHit", false);
    }
}
