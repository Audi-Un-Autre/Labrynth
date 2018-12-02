using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENEMY_FOOT : MonoBehaviour {

    public AudioSource source;
    public ENEMY_AI enemy;
	
	// Update is called once per frame
	void Update () {
        if (enemy.nav.velocity != Vector3.zero)
            source.Play();
    }
}
