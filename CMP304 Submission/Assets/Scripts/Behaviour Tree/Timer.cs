using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class Timer : Node
    {
        private float duration;
        private float time;

        public Timer(float _duration, List<Node> children)
            : base(children)
        {
            duration = _duration;
            time = 0f;
        }

        public override NodeState Evaluate()
        {
            Debug.Log("We Go Hard");
            children[0].Evaluate();
            time += Time.deltaTime;

            if (time > duration)
            {
                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.IDLE;
            return state;
        }
    }
}
