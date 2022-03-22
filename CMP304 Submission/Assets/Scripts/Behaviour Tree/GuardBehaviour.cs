using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviourTree;

public class GuardBehaviour : TreeNode
{
    public static float speed = 20f;

    public UnityEngine.Transform[] waypoints;

    Seeker seeker;

    protected override Node SetupTree()
    {
        seeker = GetComponent<Seeker>();

        Node root = new Selector(new List<Node>
        {
            new Selector(new List<Node>
            {
                new Patrol(transform, waypoints, seeker),
                new Attack(transform, seeker)
            }),

            new Sequence(new List<Node>
            {
                new MoveToAlert(transform, seeker)
            //    new Selector(new List<Node>
            //    {
            //        new Search(),
            //        new Attack(transform, seeker)
            //    })
            })
        });

        return root;
    }
}