using UnityEngine;
using System.Collections.Generic;

namespace BehaviourTree
{
    public class Timer : Node
    {
        private float _delay;
        private float time;

        public delegate void TickEnded();
        public event TickEnded onTickEnded;

        public Timer(float delay, List<Node> children, TickEnded onTickEnded = null)
            : base(children)
        {
            _delay = delay;
            time = _delay;
            this.onTickEnded = onTickEnded;
        }

        public override NodeState Evaluate()
        {
            if (time <= 0)
            {
                time = _delay;
                if (onTickEnded != null)
                    onTickEnded();
                state = NodeState.SUCCESS;
            }
            else
            {
                state = children[0].Evaluate();
                time -= Time.deltaTime;
                state = NodeState.FAILURE;
            }
            return state;
        }
    }
}
