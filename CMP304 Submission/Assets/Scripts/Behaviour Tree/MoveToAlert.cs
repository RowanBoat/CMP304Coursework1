using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class MoveToAlert : Node
{
    private Transform transform;

    private GameObject[] alert;
    private GameObject[] targets;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    float waypointDistance = 1f;

    public MoveToAlert(Transform newTransform, Seeker newSeeker)
    {
        transform = newTransform;
        seeker = newSeeker;
        targets = GameObject.FindGameObjectsWithTag("Target");
    }

    private float updateTime = 0.5f; // in seconds
    private float updateCounter = 0f;

    public override NodeState Evaluate()
    {
        alert = GameObject.FindGameObjectsWithTag("Alert2");

        updateCounter += Time.deltaTime;
        if (updateCounter >= updateTime)
        {
            UpdatePath(alert[0].transform);
            updateCounter = 0f;
        }

        if (path == null)
        {
            return NodeState.FAILURE;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return NodeState.SUCCESS;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 force = direction * GuardBehaviour.speed * Time.deltaTime;
        transform.Translate(force);
        
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        
        if (distance < waypointDistance)
        {
            currentWaypoint++;
        }
        
        float alertDistance = Vector2.Distance(alert[0].transform.position, transform.position);
        Debug.Log(alertDistance);
        if (alertDistance < 1.0f)
        {
            UnityEngine.Object.Destroy(alert[0]);
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
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
