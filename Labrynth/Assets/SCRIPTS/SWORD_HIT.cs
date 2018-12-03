using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWORD_HIT : MonoBehaviour {

    private int swordDmg = 1;
    public Animator anim;

    private void Start()
    {
        anim = transform.root.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider col){
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("basicAttack") && col.transform.tag == "Enemy"){
            Debug.Log("ENEMY HIT");
            col.GetComponent<ENEMY_AI>().currHealth -= swordDmg;

        }
    }

}
