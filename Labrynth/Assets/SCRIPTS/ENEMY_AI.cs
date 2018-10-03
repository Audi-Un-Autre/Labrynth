using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class ENEMY_AI : MonoBehaviour {

    Camera view;
    private GameObject[] wayPoints;
    NavMeshAgent nav;

    Vector3 destination;
    Vector3 playerDestination;
    Vector3 randDirection;

    private Plane[] geoPlanes;
    private Collider objectCollision;
    Rigidbody rb;

    enum GameState {IDLE, ALERTED, CHASING, PATROL, ATTACKING, DEATH};
    GameState state;

    float lastState = 0.0f;
    private bool destinationSet;
    [SerializeField] private bool playerFound;
    private float fieldOfView;

    static float IDLE_SPEED = 2;
    static float PATROL_SPEED = 5;
    static float ALERT_SPEED = 0;
    static float CHASE_SPEED = 10;
    static float SPEED_MODIFER = 1.5f;

    float waitTime = 0f;
    float maxTime = 5f;
    float idleTime = 0f;
    float maxIdleTime = 3f;
    bool timeReached = false;

	void Start () {
        nav = gameObject.GetComponent<NavMeshAgent>();

        state = GameState.IDLE;
        lastState = Time.time;

        view = gameObject.GetComponentInChildren<Camera>();
        fieldOfView = view.fieldOfView;
        objectCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
    }
	
	void Update () {
        switch (state)
        {
            case GameState.IDLE:
                if (CheckForPlayer()){
                    state = GameState.CHASING;
                    destinationSet = false;
                }
                else{
                    // GO IN A RANDOM DIRECTION WHILE IDLE
                    /*
                    nav.speed = IDLE_SPEED;
                    idleTime += Time.deltaTime;
                    if (idleTime >= maxIdleTime){
                        Idle();
                        idleTime = 0;
                        maxIdleTime = Random.Range(1, 5);
                    }*/
                }
            break;

            case GameState.ALERTED:
                waitTime += Time.deltaTime;
                if (waitTime >= maxTime){
                    timeReached = true;
                    waitTime = 0;
                    destinationSet = false;
                    state = GameState.IDLE;
                }
                else if (waitTime < maxTime) {
                    timeReached = false;
                    if (!CheckForPlayer() && !timeReached)
                        Alerted();
                    else if (CheckForPlayer() && !timeReached){
                        waitTime = 0;
                        destinationSet = false;
                        state = GameState.CHASING;
                    }
                }
            break;

            case GameState.CHASING:
                if (CheckForPlayer())
                    Chasing(playerDestination);
                else{
                    destinationSet = false;
                    state = GameState.ALERTED;   
                }
            break;

            // CREATE WAYPOINTS BASED ON LAST SEEN LOCATION *********************************************************************************************************
            case GameState.PATROL:

                break;
            
            // HANDLE ATTACK ANIMs AND HP ***************************************************************************************************************************
            case GameState.ATTACKING:

                break;
            
            // HANDLE DEATH ANIMs & MODEL FADEOUT *******************************************************************************************************************
            case GameState.DEATH:

                break;
        }
	}

    bool CheckForPlayer(){
        geoPlanes = GeometryUtility.CalculateFrustumPlanes(view);

        if (GeometryUtility.TestPlanesAABB(geoPlanes, objectCollision.bounds)){
            RaycastHit hit;
            Vector3 direction = (objectCollision.transform.position - view.transform.position).normalized;
            if (Physics.Raycast(view.transform.position, direction, out hit)){
                if (hit.transform.gameObject.name == "PLAYER"){
                    playerFound = true;
                    Debug.Log("PLAYER IN SIGHT");
                    playerDestination = objectCollision.transform.position;
                }
                else{
                    Debug.Log("PLAYER IN FRUSTRUM, BUT NOT IN SIGHT.");
                    playerFound = false;
                }
            }
            else
                playerFound = false;
        }
        else
            playerFound = false;
        return playerFound;
    }

    void Idle(){
        float x = transform.position.x;
        float z = transform.position.z;
        float _x = x + Random.Range(x - 100, x + 100);
        float _z = z + Random.Range(z - 100, z + 100);
        destination = new Vector3(_x, transform.position.y, _z);
        nav.SetDestination(destination);
        destinationSet = false;
    }

    void CheckDestination(){
        if (!nav.pathPending){
            if (nav.remainingDistance <= nav.stoppingDistance){
                if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f){
                    destinationSet = false;
                    nav.enabled = false;
                }
            }
        }
    }

    void Alerted(){
        nav.speed = ALERT_SPEED;
        Debug.Log("STATE: ALERTED.");
    }

    void Chasing(Vector3 playerPosition){
        Debug.Log("STATE: CHASING.");
        nav.speed = CHASE_SPEED;

        // QUARTERNION LERP ***********************************************************************************************************
        transform.LookAt(playerPosition);

        if (!destinationSet){
            SetDestination(playerPosition);
        }

        else if (destinationSet && gameObject.transform.position != playerPosition){
            nav.destination = playerPosition;
            CheckDestination();
        }
    }

    void SetDestination(Vector3 dest){
        nav.enabled = true;
        transform.LookAt(dest);
        destination = dest;
        destinationSet = true;
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Stairs"){
            RaycastHit hit;
            if (other.Raycast(new Ray(view.transform.position, Vector3.down), out hit, 50)){
                var slope = hit.normal;
                view.transform.rotation = Quaternion.FromToRotation(view.transform.up, hit.normal) * transform.rotation;
            }
        }
    }
    
    void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Stairs"){
            Debug.Log(other.gameObject);
            var thisRot = gameObject.transform.rotation.eulerAngles;
            view.transform.rotation = Quaternion.Euler(0, thisRot.y, thisRot.z);
        }
    }
    
}
