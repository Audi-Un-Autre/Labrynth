using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    [SerializeField] private float maxHealth;
    [SerializeField] private float currHealth;

	// Use this for initialization
	void Start () {
        currHealth = maxHealth;
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
