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

        //Node root = new Selector(new List<Node>
        //{
        //    new Selector(new List<Node>
        //    {
        //        new Patrol(transform, waypoints, seeker),
        //        new Attack()
        //    }),

        //    new Sequence(new List<Node>
        //    {
        //        new MoveToAlert(),
        //        new Selector(new List<Node>
        //        {
        //            new Search(),
        //            new Attack()
        //        }
        //    }
        //});

        return root;
    }
}