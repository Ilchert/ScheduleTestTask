using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class Graph
    {
        public List<Node> RootNodes { get; }

        public Dictionary<int, Node> AllNodes { get; }

        public int NodesCount => AllNodes.Count;

        public Graph(List<Node> rootNodes, Dictionary<int, Node> allNodes)
        {
            RootNodes = rootNodes;
            AllNodes = allNodes;
        }
    }
}
