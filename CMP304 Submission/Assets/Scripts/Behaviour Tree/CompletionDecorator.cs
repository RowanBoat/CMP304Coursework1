using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class CompletionDecorator : Node
    {
        private NodeState childState;
        private float timer;

        private GameObject[] alert;

        public CompletionDecorator(List<Node> children) : base(children) 
        {
            timer = 0f;
            childState = NodeState.RUNNING;
        }

        public override NodeState Evaluate()
        {
            alert = GameObject.FindGameObjectsWithTag("Alert2");
            while (children[0].Evaluate() != NodeState.SUCCESS)
            {
 //               childState = children[0].Evaluate();
 //               timer += Time.deltaTime;
 //               if (timer > 10f)
 //               {
 //                   timer = 0f;
 //                   childState = NodeState.RUNNING;
 //                   UnityEngine.Object.Destroy(alert[0]);
 //                   break;
 //               }
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}
