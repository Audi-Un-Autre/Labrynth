using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightshadeController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform enemy;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float specialDistance;
    [SerializeField] Rigidbody rb;
    static Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Vector3.Distance(target.position, this.transform.position) < detectionDistance) // FOLLOW DISTANCE
        {
            Vector3 direction = target.position - enemy.position;
            if (direction.magnitude < attackDistance) // RADIUS OF SATISFACTION
            {
                this.transform.Translate(0.0f, 0.0f, 0.0f);
                anim.SetBool("isAttacking", true);
                anim.SetBool("isMoving", false);
                anim.SetBool("AttackHeavy", false);
                // STOP
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                if (direction.magnitude < specialDistance)
                {
                    this.transform.Translate(0.0f, 0.0f, 0.0f);
                    anim.SetBool("AttackHeavy", true);
                    anim.SetBool("isMoving", false);
                    anim.SetBool("isAttacking", false);
                    //Stop
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
            else if (direction.magnitude > attackDistance && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                anim.SetBool("isMoving", true);
                direction.Normalize();
                direction *= moveSpeed;
                rb.velocity = direction;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.01f);
                anim.SetBool("isAttacking", false);
                anim.SetBool("AttackHeavy", false);
            }
        }
        else
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("AttackHeavy", false);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
