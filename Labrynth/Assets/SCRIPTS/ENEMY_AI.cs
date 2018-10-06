using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ENEMY_AI : MonoBehaviour {

    Camera view;
    [SerializeField] GameObject[] wayPoints;
    private GameObject wayPoint;
    NavMeshAgent nav;
    [SerializeField] Light light;

    Vector3 destination;
    Vector3 playerDestination;
    Vector3 randDirection;
    Vector3 lastSeen;

    Quaternion look;

    private Plane[] geoPlanes;
    private Collider objectCollision;
    Rigidbody rb;

    enum GameState {IDLE, ALERTED, CHASING, PATROL, ATTACKING, DEATH};
    GameState state;

    float lastState = 0.0f;
    private bool destinationSet;
    [SerializeField] private bool playerFound;
    private float fieldOfView;

    static float IDLE_SPEED = 2f;
    static float PATROL_SPEED = 1f;
    static float ALERT_SPEED = 0f;
    static float CHASE_SPEED = 10f;
    static float SPEED_MODIFER = 1.5f;
    static int _wayPoints = 4;

    float waitTime = 0f;
    float maxTime = 10f;
    float idleTime = 0f;
    float maxIdleTime = 3f;
    float lookTime = 0f;
    float maxLookTime = 3f;
    float wait = 0f;
    int currentPoint = 0;
    bool timeReached = false;
    bool waypointsSpawned;

	void Start () {
        // GRAB COMPONENTS
        nav = gameObject.GetComponent<NavMeshAgent>();
        view = gameObject.GetComponentInChildren<Camera>();
        light = GetComponent<Light>();
        
        // SET STATE
        state = GameState.IDLE;
        lastState = Time.time;
        
        // ADJUST CAMERA
        fieldOfView = view.fieldOfView;
        view.farClipPlane = 14;

        // SETUP ROATIONS
        look = Quaternion.Euler(new Vector3(0f, Random.Range(-180f, 180f), 0f));

        // SETUP PLAYER COLLISION
        objectCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        waypointsSpawned = false;
    }
	
	void Update () {

        /*
         * 
         * Escalation: Idle -> Chase -> Attack
         * Descalation: Alert -> Patrol
         * 
         */
        switch (state)
        {
            case GameState.IDLE:
                light.color = Color.white;
                if (CheckForPlayer()){
                    state = GameState.CHASING;
                    destinationSet = false;
                }
                else{
                    // GO IN A RANDOM DIRECTION WHILE IDLE
                    nav.speed = IDLE_SPEED;
                    idleTime += Time.deltaTime;
                    if (idleTime >= maxIdleTime){
                        Idle();
                        idleTime = 0;
                        maxIdleTime = Random.Range(1, 5);
                    }
                }
                break;

            case GameState.CHASING:
                light.color = Color.red;
                if (CheckForPlayer())
                    Chasing(playerDestination);
                else{
                    destinationSet = false;
                    state = GameState.ALERTED;
                }
                break;

            case GameState.ALERTED:
                // IDLE AT THE SPOT

                light.color = Color.yellow;
                waitTime += Time.deltaTime;
                if (waitTime >= maxTime)
                {
                    timeReached = true;
                    waitTime = 0;
                    destinationSet = false;
                    wait = 0;
                    state = GameState.PATROL;
                }
                else if (waitTime < maxTime)
                {
                    timeReached = false;
                    if (!CheckForPlayer() && !timeReached)
                        Alerted();
                    else if (CheckForPlayer() && !timeReached)
                    {
                        waitTime = 0;
                        destinationSet = false;
                        wait = 0;
                        state = GameState.CHASING;
                    }
                }

                // LOOK AROUND FOR PLAYER
                wait += Time.deltaTime;
                lookTime += Time.deltaTime;
                if (lookTime >= maxLookTime)
                {
                    look = Quaternion.Euler(new Vector3(0f, Random.Range(-180f, 180f), 0f));
                    lookTime = 0;
                }
                // CONTINUE LOOKING IN DIRECTION LAST SEEN PLAYER FOR 1.5s
                if (wait > 1.5f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * 5f);
                    transform.Translate(Vector3.forward * ALERT_SPEED * Time.deltaTime);
                }
                break;

            // CREATE WAYPOINTS BASED ON LAST SEEN LOCATION ****
            case GameState.PATROL:
                light.color = Color.blue;
                bool called = false;
                if (CheckForPlayer()){
                    PatrolDone();
                    state = GameState.CHASING;
                }
                else if (!waypointsSpawned && !called){
                    called = true;
                    CreateWaypoints();
                }
                else if (waypointsSpawned && Patrol()){
                    PatrolDone();
                    state = GameState.IDLE;
                }
                break;

            // HANDLE ATTACK ANIMs AND HP *********************
            case GameState.ATTACKING:
                light.color = Color.red;
                break;

            // HANDLE DEATH ANIMs & MODEL FADEOUT *************
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

    void CreateWaypoints(){
        float distanceFromOrigin = 10f;
        Vector3 origin = playerDestination;
        Vector3 waypointSet = playerDestination;

        // CREATE THE WAYPOINTS WITH THE ORIGIN AT THE PLAYER"S LAST SEEN LOCATION
        for (int i = 0; i < _wayPoints; i++){
            wayPoint = new GameObject("WAYPOINT " + (i + 1));
            wayPoint.tag = "Waypoint";
            float point = (i * 1.0f) / _wayPoints;
            float angle = point * Mathf.PI * 2;
            float x = Mathf.Sin(angle) * distanceFromOrigin;
            float y = Mathf.Cos(angle) * distanceFromOrigin;
            wayPoint.transform.position = new Vector3(x, 0, y) + playerDestination;
            wayPoint.transform.parent = this.transform;
        }

        // ADD CHILD WAYPOINTS TO ARRAY
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Waypoint"))
                wayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        }

        waypointsSpawned = true;
    }

    bool Patrol()
    {
        Debug.Log("STATE: PATROL.");
        nav.speed = PATROL_SPEED;

        // SET CURRENT DESTINATION IN WAYPONT ARRAY
        if (currentPoint != _wayPoints){
            if (!destinationSet){
                SetDestination(wayPoints[currentPoint].transform.position);
                currentPoint++;
            }

            // GO TO DESTINATION
            else if (destinationSet && gameObject.transform.position != destination){
                nav.destination = destination;
                CheckDestination();
            }
            return false;
        }
        return true;
    }

    void PatrolDone(){
        // REMOVE WAYPOINTS

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Waypoint"))
                GameObject.Destroy(child.gameObject);
        }
        waypointsSpawned = false;
    }

    void CheckDestination(){

        if (waypointsSpawned){
            // CHECK IF PATH IS PATHABLE, ex: waypoint spawned in an obstacle means waypoint is not pathable
            Vector3 spawnLocation = wayPoints[currentPoint].transform.position;
            NavMeshPath path = new NavMeshPath();
            nav.CalculatePath(nav.destination, path);

            if (path.status != NavMeshPathStatus.PathComplete){
                destinationSet = false;
                nav.isStopped = true;
                nav.ResetPath();
            }
        }

        // CHECK IF PATH COMPLETED
        if (!nav.pathPending){
            if (nav.remainingDistance <= nav.stoppingDistance){
                if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f){
                    destinationSet = false;
                    nav.isStopped = true;
                    nav.ResetPath();
                }
            }
        }
    }

    // TURN AROUND IN A RANDOM DIRECTION WHILE IN PLACE
    void Alerted(){
        nav.speed = ALERT_SPEED;
        Debug.Log("STATE: ALERTED.");
    }

    void Chasing(Vector3 playerPosition){
        Debug.Log("STATE: CHASING.");
        nav.speed = CHASE_SPEED;

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
