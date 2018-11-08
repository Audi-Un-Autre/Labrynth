using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    private static float maxHealth;
    private static float currHealth;

	// Use this for initialization
	void Start () {
        maxHealth = 10;
        currHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void HurtPlayer(int damage)
    {
        currHealth -= damage;
        print("Current health is: " + currHealth);
    }

    public static void HealPlayer(int healAmount)
    {
        currHealth += healAmount;
        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

    }
}
