// Guard AI Finite State Machine
// By Rowan Ruthven
// 1802152

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GuardFSM : MonoBehaviour
{
    // All of the states the AI will be using
    public enum GuardState 
    { 
        Patrol, 
        Attack, 
        Alert, 
        Search 
    }

    GuardState state;

    // Movement Variables/Objects
    public static float speed = 20f;
    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    float waypointDistance = 1f;
    public Transform[] waypoints;
    private Transform currentDestination;
    float targetDistance;
    Vector2 direction;
    Vector2 force;
    float distance;

    // Setting up for tags
    private GameObject[] alert;
    private GameObject[] targets;
    private GameObject target;

    private float waitTime = 1f; // in seconds
    private float waitCounter = 0f;
    private bool waiting = false;

    float searchTime = 0f;

    private int currentWaypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        targets = GameObject.FindGameObjectsWithTag("Target");
        InvokeRepeating("UpdatePath", 0.0f, 0.5f);

        state = GuardState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case GuardState.Patrol:
                alertCheck();

                if (waiting)
                {
                    waitCounter += Time.deltaTime;
                    if (waitCounter >= waitTime)
                    {
                        waiting = false;
                    }
                }
                else
                {
                    Transform wp = waypoints[currentWaypointIndex];
                    currentDestination = wp;

                    if (Vector3.Distance(transform.position, wp.position) <= 1f)
                    {
                        transform.position = wp.position;
                        waitCounter = 0f;
                        waiting = true;

                        currentWaypointIndex++;
                        if (currentWaypointIndex >= waypoints.Length)
                            currentWaypointIndex = 0;
                    }
                    else
                    {
                        lookForTargets(0f);

                        moveAlongPath();
                    }
                }
                break;

            case GuardState.Alert:
                alert = GameObject.FindGameObjectsWithTag("Alert");
                currentDestination = alert[0].transform;
                moveAlongPath();

                float alertDistance = Vector2.Distance(alert[0].transform.position, transform.position);
                if (alertDistance < 1.0f)
                {
                    UnityEngine.Object.Destroy(alert[0]);
                    state = GuardState.Search;
                    break;
                }
                break;

            case GuardState.Search:
                alertCheck();
                searchTime += Time.deltaTime;

                lookForTargets(searchTime);

                if (searchTime >= 5f)
                {
                    searchTime = 0f;
                    state = GuardState.Patrol;
                    break;
                }
                break;

            case GuardState.Attack:
                alertCheck();
                findClosestTarget();
                currentDestination = target.transform;
                moveAlongPath();

                targetDistance = Vector2.Distance(target.transform.position, transform.position);
                if (targetDistance < 1.0f)
                {
                    UnityEngine.Object.Destroy(target);
                    state = GuardState.Patrol;
                    break;
                }
                break;
        }
    }

    void findClosestTarget()
    {
        // When in the Attack state, checks which target is closer to the guard object
        int closest = -1;
        float distance = Mathf.Infinity;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
            {
                Vector3 difference = targets[i].transform.position - transform.position;
                float currentDistance = difference.sqrMagnitude;
                if (currentDistance < distance)
                {
                    closest = i;
                    distance = currentDistance;
                }
            }
        }
        target = targets[closest];
    }

    // Pathfinding functions
    void UpdatePath()
    {
        if (currentDestination != null)
            if (seeker.IsDone()) 
                seeker.StartPath(transform.position, currentDestination.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void alertCheck()
    {
        // Checks if any alerts are in the scene
        alert = GameObject.FindGameObjectsWithTag("Alert");
        if (alert.Length > 0)
        {
            state = GuardState.Alert;
        }
    }

    void lookForTargets(float rad)
    {
        // Checks in a radius whether or not there are any targets nearby
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
            {
                targetDistance = Vector2.Distance(targets[i].transform.position, transform.position);
                if (targetDistance < 10.0f + rad)
                {
                    state = GuardState.Attack;
                    break;
                }
            }
        }
    }

    void moveAlongPath()
    {
        // Takes the current path and moves the object along it
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Calculating a normalised direction from point A (current position) and point B (waypoint)
        direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
        force = direction * speed * Time.deltaTime;
        transform.Translate(force);

        distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        // Moves to next waypoint on the path once object is close enough to the current waypoint
        if (distance < waypointDistance)
        {
            currentWaypoint++;
        }
    }
}