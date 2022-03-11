using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class MoveToAlert : Node
{
    private Transform transform;
    private Transform alert;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    float waypointDistance = 1f;

    public MoveToAlert(Transform newTransform, Transform newAlert, Seeker newSeeker)
    {
        transform = newTransform;
        alert = newAlert;
        seeker = newSeeker;
        UpdatePath(alert);
    }

    public override NodeState Evaluate()
    {
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
