using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class Patrol : Node
{
    private Transform transform;
    private Transform[] waypoints;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    bool reachedEnd = false;
    float waypointDistance = 1f;

    public Patrol(Transform newTransform, Transform[] newWaypoints, Seeker newSeeker)
    {
        transform = newTransform;
        waypoints = newWaypoints;
        seeker = newSeeker;
        UpdatePath(waypoints[3]);
    }

    private int currentWaypointIndex = 0;

    private float waitTime = 1f; // in seconds
    private float waitCounter = 0f;
    private bool waiting = false;

    private float updateTime = 0.5f; // in seconds
    private float updateCounter = 0f;

    private Vector2 debug;

    public override NodeState Evaluate()
    {
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
            if (Vector3.Distance(transform.position, wp.position) <= 1f)
            {
                transform.position = wp.position;
                waitCounter = 0f;
                waiting = true;

                //currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                currentWaypointIndex++;
                Debug.Log("Current Waypoint Location:" + currentWaypointIndex);
                if (currentWaypointIndex >= waypoints.Length)
                    currentWaypointIndex = 0;
            }
            else
            {
                // transform.position = Vector3.MoveTowards(
                //     transform.position,
                //     wp.position,
                //     GuardBehaviour.speed * Time.deltaTime);

                updateCounter += Time.deltaTime;
                if (updateCounter >= waitTime)
                {
                    UpdatePath(wp);
                    updateCounter = 0f;
                }

                if (path == null)
                    return NodeState.FAILURE;

                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEnd = true;
                    return NodeState.SUCCESS;
                }
                else
                {
                    reachedEnd = false;
                }

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
                Vector2 force = direction * GuardBehaviour.speed * Time.deltaTime;
                debug = (Vector2)path.vectorPath[currentWaypoint];
                transform.Translate(force);

                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

                if (distance < waypointDistance)
                {
                    currentWaypoint++;
                }
            }
        }

        //Debug.Log("Running (" + debug + ")");
        state = NodeState.RUNNING;
        return state;
    }

    void UpdatePath(Transform waypoint)
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, waypoint.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}

