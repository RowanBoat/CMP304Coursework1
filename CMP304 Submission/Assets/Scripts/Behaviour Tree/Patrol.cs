using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class Patrol : Node
{
    private Transform transform;
    private Transform[] waypoints;

    private GameObject[] targets;

    private GameObject[] alert;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    float waypointDistance = 1f;

    public Patrol(Transform newTransform, Transform[] newWaypoints, Seeker newSeeker)
    {
        transform = newTransform;
        waypoints = newWaypoints;
        seeker = newSeeker;
        targets = GameObject.FindGameObjectsWithTag("Target");
        UpdatePath(waypoints[3]);
    }

    private int currentWaypointIndex = 0;

    private float waitTime = 1f; // in seconds
    private float waitCounter = 0f;
    private bool waiting = false;

    private float updateTime = 0.5f; // in seconds
    private float updateCounter = 0f;

    public override NodeState Evaluate()
    {
        alert = GameObject.FindGameObjectsWithTag("Alert");
        if (alert.Length > 0)
        {
            state = NodeState.FAILURE;
            updateCounter = updateTime;
            return state;
        }

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

                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                    currentWaypointIndex = 0;
            }
            else
            {
                updateCounter += Time.deltaTime;
                if (updateCounter >= updateTime)
                {
                    UpdatePath(wp);
                    updateCounter = 0f;
                }

                if (path == null)
                    return NodeState.FAILURE;

                if (currentWaypoint >= path.vectorPath.Count)
                    return NodeState.SUCCESS;

                for (int i = 0; i < targets.Length; i++)
                {
                    if (targets[i] != null)
                    {
                        float targetDistance = Vector2.Distance(targets[i].transform.position, transform.position);
                        if (targetDistance < 10.0f)
                        {
                            state = NodeState.FAILURE;
                            updateCounter = updateTime;
                            return state;
                        }
                    }
                }

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
                Vector2 force = direction * GuardBehaviour.speed * Time.deltaTime;
                transform.Translate(force);

                float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

                if (distance < waypointDistance)
                {
                    currentWaypoint++;
                }
            }
        }

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

