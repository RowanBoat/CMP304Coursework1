using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BehaviourTree
{
    public abstract class TreeNode : MonoBehaviour
    {
        private Node root = null;

        public List<float> timer;
        public float tempTimer;

        private NodeState rootState;

        protected void Start()
        {
            root = SetupTree();
            tempTimer = 0f;
        }

        private void Update()
        {
            if (root != null)
                rootState = root.Evaluate();
            tempTimer += Time.deltaTime;    
            
            if (rootState == NodeState.SUCCESS)
            {
                timer.Add(tempTimer);
                WriteCSV();
                tempTimer = 0f;
            }
        }

        protected abstract Node SetupTree();

        public void WriteCSV()
        {                
            if(timer.Count > 0)
            {
                TextWriter tw = new StreamWriter(Application.dataPath + "/testBT.csv", false);
                tw.WriteLine("Time");
                tw.Close();

                tw = new StreamWriter(Application.dataPath + "/testBT.csv", true);

                for(int i = 0; i < timer.Count; i++)
                {
                    tw.WriteLine(timer[i].ToString());
                }
                tw.Close();
            }
        }
    }
}