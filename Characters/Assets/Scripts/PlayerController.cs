using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    [SerializeField] private float runMultiplier;
    [SerializeField] private int damage = 1;
    private static float maxHealth;
    private static float currHealth;
    private CharacterController character;
    private Vector3 movement;
    static Animator anim;

    // Use this for initialization
    void Start ()
    {
        maxHealth = 10;
        currHealth = maxHealth;
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void LateUpdate ()
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
            movement = (transform.right * -Input.GetAxis("Horizontal"));
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
            movement = (transform.right * -Input.GetAxis("Horizontal"));
            movement = movement.normalized * moveSpeed;
            character.Move(movement * Time.deltaTime);
        }
        else
        {
            anim.SetBool("runRight", false);
        }

        if(currHealth <= 0)
        {
            anim.SetBool("isDead", true);
            moveSpeed = 0.0f;
        }
    }

    public static void HurtPlayer(int damage)
    {
        currHealth -= damage;
        print("Current health is: " + currHealth);
        //takeDamage();
        //anim.SetBool("isHit", false);
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

    public void EnemyHit()
    {
        EnemyController.HurtEnemy(damage);
    }
}

// 2. Sword push enemies
// 6. Stop moving when attacking