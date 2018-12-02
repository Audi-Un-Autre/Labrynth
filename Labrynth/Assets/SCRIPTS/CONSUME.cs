using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONSUME : MonoBehaviour {

    public PlayerController health;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "POTION"){ }
            //consume, and play sound
        if (collision.transform.tag == "FOOD"){ }
            //consume, and play sound
    }
}
