using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class GuardBehaviour : TreeNode
{
    public static float speed = 50f;
    Vector3 mousePos;
    Vector3 alertPos;

    public UnityEngine.Transform[] waypoints;

    Seeker seeker;

    protected override Node SetupTree()
    {
        seeker = GetComponent<Seeker>();
        Node root = new Patrol(transform, waypoints, seeker);
        return root;
    }

    //void UpdatePath()
    //{
    //    if (seeker.IsDone())
    //        seeker.StartPath(transform.position, alertPos, OnPathComplete);
    //}

    //void OnPathComplete(Path p)
    //{
    //    if (!p.error)
    //    {
    //        path = p;
    //        currentWaypoint = 0;
    //    }
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        time = Time.time;
    //        alertPos = mousePos;
    //    }

    //    if (path == null)
    //        return;

    //    if (currentWaypoint >= path.vectorPath.Count)
    //    {
    //        reachedEnd = true;
    //        return;
    //    }
    //    else
    //    {
    //        reachedEnd = false;
    //    }

    //    Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
    //    Vector2 force = direction * speed * Time.deltaTime;

    //    transform.Translate(force);

    //    float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);

    //    if (distance < waypointDistance)
    //    {
    //        currentWaypoint++;
    //    }
    //}
}