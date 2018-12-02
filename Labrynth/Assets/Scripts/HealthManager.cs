using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public float maxHealth;
    public float currHealth;

	// Use this for initialization
	void Start () {
        currHealth = 10;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HurtPlayer(int damage)
    {
        currHealth -= damage;
    }


    public void HealPlayer(int healAmount)
    {
        currHealth += healAmount;
        if (currHealth > maxHealth)
        {
            currHealth = maxHealth;
        }

    }
}
