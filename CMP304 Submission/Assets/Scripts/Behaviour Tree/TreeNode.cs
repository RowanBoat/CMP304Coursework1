using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BehaviourTree
{
    public abstract class TreeNode : MonoBehaviour
    {
        private Node root = null;

        protected void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if (root != null)
                root.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}