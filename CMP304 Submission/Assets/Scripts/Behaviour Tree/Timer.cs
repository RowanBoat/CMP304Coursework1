using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Decorator Node does not work, after many iterations.
/// </summary>
namespace BehaviourTree
{
    public class Timer : Node
    {
        float runTime;
        float elapsedTime = 0f;

        public Timer(float duration, List<Node> children) : base(children)
        {
            runTime = duration;            
        }

        public override NodeState Evaluate()
        {
            // Update elapsed time
            elapsedTime += Time.deltaTime;

            // If the node is allowed to run
            if (elapsedTime < runTime)
            {
                // Run the Node and get its state
                NodeState childState = children[0].Evaluate();

                // If the child node stops running, reset the timer and update the decorator with the child nodes state
                if (childState != NodeState.RUNNING)
                {
                    state = childState;
                    elapsedTime = 0f;
                }
            }
            else
            {
                // If we run out of time, assume node is successful and force this state into the child node
                state = NodeState.SUCCESS;
                elapsedTime = 0f;
            }

            return state;
        }
    }
}
