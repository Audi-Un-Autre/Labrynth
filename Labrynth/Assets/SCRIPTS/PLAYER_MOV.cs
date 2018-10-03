using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYER_MOV : MonoBehaviour {

    Vector3 targetDestination;
    Vector3 playerPosition;
    Vector3 destination;
    [SerializeField] Rigidbody rb;

    [SerializeField] bool clicked;
    float maxSpeed;
    float stopRadius;

    void Start()
    {

        stopRadius = 2f;
        maxSpeed = 12f;
    }

    void Update()
    {

        MouseClicked();

        if (clicked)
        {
            SetDestination();
            GoToDestination();
        }

    }

    // CHECK IF PLAYER IS CLICKING
    private void MouseClicked()
    {

        // GET MOUSE CLICK POSITION WHEN CLICKED, IF CLICKED ON THE FLOOR
        RaycastHit hit;
        if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            clicked = true;
            if (hit.collider.tag == "Floor")
            {
                targetDestination = hit.point;
            }
        }
    }

    // MOVE PLAYER TO CLICK LOCATION
    private void SetDestination()
    {

        // CREATE PATH TO DESTINATION
        destination = targetDestination - transform.position;
        transform.LookAt(targetDestination);
    }

    // CHECK IF ARRIVED TO DESTINATION
    private void GoToDestination()
    {

        // MOVE
        Debug.Log(destination.magnitude);
        if (destination.magnitude > stopRadius)
        {
            destination.Normalize();
            destination *= maxSpeed;
            rb.velocity = destination;
        }

        // STOP
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            clicked = false;
        }
    }
}
