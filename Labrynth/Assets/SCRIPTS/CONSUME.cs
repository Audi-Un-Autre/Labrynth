using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONSUME : MonoBehaviour {

    public PlayerController track;
    private int healthAmt = 5;
    private int foodAmt = 1;
    public bool consumed = false;
    

    private void OnTriggerStay(Collider col)
    {
        if (!consumed){
            //POTION
            if (col.transform.tag == "Potion" && Input.GetKeyDown(KeyCode.E)){ 
                if (PlayerController.currHealth != PlayerController.maxHealth && !(PlayerController.currHealth + healthAmt > PlayerController.maxHealth)){
                    consumed = true;
                    PlayerController.currHealth += healthAmt;
                    Destroy(col.gameObject);
                }
                else
                    Debug.Log("HEALTH FULL");

                /*
                // TRACK HEALTH TEST
                if (track.trackHealth != PlayerController.maxHealth && !(track.trackHealth + healthAmt > PlayerController.maxHealth))
                {
                    consumed = true;
                    track.trackHealth += healthAmt;
                    Destroy(col.gameObject);
                }
                else
                    Debug.Log("HEALTH FULL");
                    */
            }
        
            // FOOD
            if (col.transform.tag == "Food" && Input.GetKeyDown(KeyCode.E)){
                if (PlayerController.currHealth != PlayerController.maxHealth && !(PlayerController.currHealth + foodAmt > PlayerController.maxHealth)){
                    consumed = true;
                    PlayerController.currHealth += foodAmt;
                    Destroy(col.gameObject);
                }
                else
                    Debug.Log("HEALTH FULL");

                // TRACK HEALTH TEST
                /*
                if (track.trackHealth != PlayerController.maxHealth && !(track.trackHealth + healthAmt > PlayerController.maxHealth))
                {
                    consumed = true;
                    track.trackHealth += foodAmt;
                    Destroy(col.gameObject);
                }
                else
                    Debug.Log("HEALTH FULL");
                    */
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        consumed = false;
    }
}
