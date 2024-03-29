using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class SearchArea : Node
{
    private Transform transform;
    private GameObject[] targets;
    private GameObject[] alert;
    float searchTime = 0f;

    public SearchArea(Transform newTransform)
    {
        transform = newTransform;
    }

    public override NodeState Evaluate()
    {
        alert = GameObject.FindGameObjectsWithTag("Alert2");
        if (alert.Length > 0)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        
        searchTime += Time.deltaTime;
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
            {
                float targetDistance = Vector2.Distance(targets[i].transform.position, transform.position);
                if (targetDistance < 10.0f + searchTime)
                {
                    searchTime = 0f;
                    state = NodeState.FAILURE;
                    return state;
                }
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
