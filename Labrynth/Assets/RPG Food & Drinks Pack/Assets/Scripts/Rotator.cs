using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    Transform[] weapons;
    float rotateSpeed = 150f;

	void Start () {
        weapons = new Transform[transform.childCount];
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = transform.GetChild(i);
        }
	}
	
	void Update () {

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.World);
        }
	}
}
