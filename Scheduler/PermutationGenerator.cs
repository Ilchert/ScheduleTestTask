using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class PermutationGenerator
    {
        private readonly Graph _dependenciesGraph;

        private readonly Graph _permutationGraph;

        public PermutationGenerator(Graph dependenciesGraph)
        {
            _dependenciesGraph = dependenciesGraph;
            _permutationGraph = GeneratePermutationGraph(dependenciesGraph);
        }

        private Graph GeneratePermutationGraph(Graph dependenciesGraph)
        {
            var allNodes = dependenciesGraph.AllNodes;
            var newNodes = allNodes.Values.Select(p => new Node(p.Id)).ToDictionary(p => p.Id);

            foreach (var fromNode in newNodes.Values)
            {
                var originalFrom = dependenciesGraph.AllNodes[fromNode.Id];
                foreach (var toNode in newNodes.Values)
                {
                    if (fromNode.Id == toNode.Id)
                        continue;

                    var originalToNode = dependenciesGraph.AllNodes[toNode.Id];
                    fromNode.Children.Add(toNode);
                }
            }

            return new Graph(newNodes.Values.ToList(), newNodes);
        }

        public IEnumerable<IReadOnlyList<Node>> Generate()
        {
            var result = new List<IReadOnlyList<Node>>();
            Visit(_permutationGraph.RootNodes, () => result.Add(_currentPath.ToArray()));
            return result;
        }

        private readonly Stack<Node> _currentPath = new Stack<Node>();
        private readonly HashSet<int> _visitedNodes = new HashSet<int>();

        private void Visit(List<Node> nodes, Action pathConstructed)
        {
            foreach (var node in nodes)
            {
                if (_visitedNodes.Contains(node.Id))
                    continue;

                _visitedNodes.Add(node.Id);
                _currentPath.Push(node);

                if (_currentPath.Count == _permutationGraph.AllNodes.Count)
                {
                    pathConstructed();
                }
                else
                {
                    Visit(node.Children, pathConstructed);
                }

                _visitedNodes.Remove(node.Id);
                _currentPath.Pop();
            }
        }
    }
}
