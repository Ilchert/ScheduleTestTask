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



        public IEnumerable<IReadOnlyList<Node>> Generate() => new PermutationGeneratorVisitor(_permutationGraph).Generate();

        private class PermutationGeneratorVisitor : InDeepVisitor
        {
            private readonly List<IReadOnlyList<Node>> _permutations = new List<IReadOnlyList<Node>>();

            public PermutationGeneratorVisitor(Graph graph) : base(graph)
            {
            }

            public IReadOnlyList<IReadOnlyList<Node>> Generate()
            {
                Visit();
                return _permutations.AsReadOnly();
            }

            protected override void PathConstructed()
            {
                _permutations.Add(GetCurrentPath());
            }
        }


        private abstract class InDeepVisitor
        {
            private readonly Graph _graph;
            private readonly Stack<Node> _currentPath = new Stack<Node>();
            private readonly HashSet<int> _visitedNodes = new HashSet<int>();

            protected InDeepVisitor(Graph graph) => _graph = graph;

            protected void Visit()
            {
                Visit(_graph.RootNodes);
            }

            private void Visit(List<Node> nodes)
            {
                foreach (var node in nodes)
                {
                    if (_visitedNodes.Contains(node.Id))
                        continue;

                    _visitedNodes.Add(node.Id);
                    _currentPath.Push(node);
                    ProcessNode(node);

                    if (_currentPath.Count == _graph.NodesCount)
                    {
                        PathConstructed();
                    }
                    else
                    {
                        Visit(node.Children);
                    }

                    _visitedNodes.Remove(node.Id);
                    _currentPath.Pop();
                }
            }

            protected IReadOnlyList<Node> GetCurrentPath() => _currentPath.Reverse().ToArray();

            protected virtual void PathConstructed()
            {

            }


            protected virtual void ProcessNode(Node node)
            {

            }
        }
    }
}
