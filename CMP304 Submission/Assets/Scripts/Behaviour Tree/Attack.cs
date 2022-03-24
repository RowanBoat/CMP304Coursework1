using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class Attack : Node
{
    private Transform transform;

    private GameObject target;
    private GameObject[] targets;

    private GameObject[] alert;

    private float updateTime = 0.5f; // in seconds
    private float updateCounter = 0f;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    float waypointDistance = 1f;

    public Attack(Transform newTransform, Seeker newSeeker)
    {
        transform = newTransform;
        seeker = newSeeker;

        targets = GameObject.FindGameObjectsWithTag("Target");
        findClosestTarget();

        UpdatePath(target.transform);
    }

    public override NodeState Evaluate()
    {
        alert = GameObject.FindGameObjectsWithTag("Alert");
        if (alert.Length > 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        if (target == null)
        {
            findClosestTarget();
        }

        updateCounter += Time.deltaTime;
        if (updateCounter >= updateTime)
        {
            UpdatePath(target.transform);
            updateCounter = 0f;
        }

        if (path == null)
            return NodeState.FAILURE;

        if (currentWaypoint >= path.vectorPath.Count)
            return NodeState.SUCCESS;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 force = direction * GuardBehaviour.speed * Time.deltaTime;
        transform.Translate(force);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (distance < waypointDistance)
        {
            currentWaypoint++;
        }

        float targetDistance = Vector2.Distance(target.transform.position, transform.position);
        if (targetDistance < 1.0f)
        {
            UnityEngine.Object.Destroy(target);
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.RUNNING;
        return state;
    }

    void findClosestTarget()
    {
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

