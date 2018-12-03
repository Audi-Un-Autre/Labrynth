using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONSUME : MonoBehaviour {

    public PlayerController health;
    private int healthAmt = 5;
    private int foodAmt = 1;
    public bool consumed = false;
    public AudioSource source;


    private void OnTriggerStay(Collider col)
    {
        if (!consumed){
            //POTION
            if (col.transform.tag == "Potion" && Input.GetKeyDown(KeyCode.E)){ 
                if (health.currHealth != health.maxHealth && !(health.currHealth + healthAmt > health.maxHealth)){
                    consumed = true;
                    health.currHealth += healthAmt;
                    Destroy(col.gameObject);
                    source.Play();
                }
                else
                    Debug.Log("HEALTH FULL");
            }
        
            // FOOD
            if (col.transform.tag == "Food" && Input.GetKeyDown(KeyCode.E)){
                if (health.currHealth != health.maxHealth && !(health.currHealth + foodAmt > health.maxHealth)){
                    consumed = true;
                    health.currHealth += foodAmt;
                    Destroy(col.gameObject);
                    source.Play();

                }
                else
                    Debug.Log("HEALTH FULL");

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        consumed = false;
    }
}
