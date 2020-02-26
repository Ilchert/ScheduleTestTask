using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class PermutationGenerator
    {
        private readonly Graph _permutationGraph;
        private readonly Stack<Node> _currentPath = new Stack<Node>();
        private readonly HashSet<int> _visitedNodes = new HashSet<int>();

        public PermutationGenerator(Graph dependenciesGraph)
        {
            _permutationGraph = GeneratePermutationGraph(dependenciesGraph);
            new PermutationOptimizerVisitor(_permutationGraph, dependenciesGraph).Visit();
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

        public IEnumerable<IReadOnlyList<Node>> Generate() => new PermutationGeneratorVisitor(_permutationGraph).Generate();

        private class PermutationOptimizerVisitor : InDeepVisitor
        {
            private readonly Graph _permutationGraph;

            public PermutationOptimizerVisitor(Graph permutationGraph, Graph dependenciesGraph) : base(dependenciesGraph)
            {
                _permutationGraph = permutationGraph;
            }

            protected override void ProcessNode(Node node)
            {
                var permutationNode = _permutationGraph.AllNodes[node.Id];
                permutationNode.Children.RemoveAll(p => VisitedNodes.Contains(p.Id));

                var hashSet = new HashSet<Node>();

                if (CurrentPath.Count > 2)
                {
                    foreach (var transitiveParentNode in CurrentPath.Skip(2))
                    {
                        var permutationTransitiveParentNode = _permutationGraph.AllNodes[transitiveParentNode.Id];
                        permutationTransitiveParentNode.Children.RemoveAll(p => p.Id == node.Id);
                    }
                }
            }

        }

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
                _permutations.Add(CurrentPath.Reverse().ToArray());
            }
        }


        private abstract class InDeepVisitor
        {
            protected Stack<Node> CurrentPath { get; } = new Stack<Node>();

            protected HashSet<int> VisitedNodes { get; } = new HashSet<int>();

            private readonly Graph _graph;

            protected InDeepVisitor(Graph graph) => _graph = graph;

            public void Visit()
            {
                Visit(_graph.RootNodes);
            }

            private void Visit(List<Node> nodes)
            {
                foreach (var node in nodes)
                {
                    if (VisitedNodes.Contains(node.Id))
                        continue;

                    VisitedNodes.Add(node.Id);
                    CurrentPath.Push(node);
                    ProcessNode(node);

                    if (CurrentPath.Count == _graph.NodesCount)
                    {
                        PathConstructed();
                    }
                    else
                    {
                        Visit(node.Children);
                    }

                    VisitedNodes.Remove(node.Id);
                    CurrentPath.Pop();
                }
            }

            protected virtual void PathConstructed()
            {
            }


            protected virtual void ProcessNode(Node node)
            {

            }
        }
    }
}
