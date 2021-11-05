using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nameless.DataMono
{
    public class Path : MonoBehaviour
    {
        
        public GameObject[] nodes;

        private float distance = 0;

        private void Start()
        {
            for(int i = 0; i < nodes.Length - 1; i++)
            {
                this.distance = this.distance + Vector3.Distance(this.nodes[i].transform.position, this.nodes[i + 1].transform.position); 
            }
        }


        public float Distance()
        {
            return this.distance;
        }
    }
}