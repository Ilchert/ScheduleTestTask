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
        private readonly Stack<Node> _currentPath = new Stack<Node>();
        private readonly HashSet<int> _visitedNodes = new HashSet<int>();

        public PermutationGenerator(Graph dependenciesGraph)
        {
            _dependenciesGraph = dependenciesGraph;
            _permutationGraph = GeneratePermutationGraph(dependenciesGraph);

            OptimizePermutations(_dependenciesGraph.RootNodes);
        }

        private Graph GeneratePermutationGraph(Graph dependenciesGraph)
        {
            var newNodes = dependenciesGraph.AllNodes.Values.Select(p => new Node(p.Id)).ToDictionary(p => p.Id);

            foreach (var fromNode in newNodes.Values)
            {
                foreach (var toNode in newNodes.Values)
                {
                    if (fromNode.Id == toNode.Id)
                        continue;

                    fromNode.Children.Add(toNode);
                }
            }

            var rootNodes = dependenciesGraph.RootNodes.Select(p => newNodes[p.Id]).ToList();

            return new Graph(rootNodes, newNodes);
        }



        private void OptimizePermutations(List<Node> rootNodes)
        {
            foreach (var node in rootNodes)
            {
                var permutationNode = _permutationGraph.AllNodes[node.Id];
                permutationNode.Children.RemoveAll(p => _visitedNodes.Contains(p.Id));

                if (_currentPath.Count > 1)
                {
                    foreach (var transitiveParentNode in _currentPath.Skip(1))
                    {
                        var permutationTransitiveParentNode = _permutationGraph.AllNodes[transitiveParentNode.Id];
                        permutationTransitiveParentNode.Children.RemoveAll(p => p.Id == node.Id);
                    }
                }

                _visitedNodes.Add(node.Id);
                _currentPath.Push(node);
                OptimizePermutations(node.Children);
                _visitedNodes.Remove(node.Id);
                _currentPath.Pop();
            }
        }



        public IEnumerable<IReadOnlyList<Node>> Generate()
        {
            var result = new List<IReadOnlyList<Node>>();
            _visitedNodes.Clear();
            _currentPath.Clear();
            Visit(_permutationGraph.RootNodes, () => result.Add(_currentPath.Reverse().ToArray()));
            return result;
        }


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
