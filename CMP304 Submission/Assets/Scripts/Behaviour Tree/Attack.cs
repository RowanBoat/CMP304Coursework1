using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class Attack : Node
{
    private Transform transform;
    private Transform target;

    Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    float waypointDistance = 1f;

    public Attack(Transform newTransform, Transform newTarget, Seeker newSeeker)
    {
        transform = newTransform;
        target = newTarget;
        seeker = newSeeker;
        UpdatePath(target);
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

