using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool useOffsetVals;
    [SerializeField] private float rotateSpeed;
    float curZoomPos, zoomTo; 
    float zoomFrom = 20f;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    // Use this for initialization
    void Start ()
    {
        
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        if (!useOffsetVals)
        {
            offset = target.position - transform.position;
        }
	}
    // Update is called once per frame
    void FixedUpdate ()
    {
        //rotates player
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, horizontal, 0);
        float yAngle = target.eulerAngles.y;

        //Changes and rotates the player based on camera
        Quaternion camTurnAngle = Quaternion.Euler(0, yAngle, 0);
        transform.position = target.position - (camTurnAngle * offset);
        transform.LookAt(target);
    }
}
