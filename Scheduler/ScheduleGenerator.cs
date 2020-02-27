using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class ScheduleGenerator
    {
        private readonly Graph _dependenciesGraph;
        private readonly Graph _permutationGraph;

        public ScheduleGenerator(Graph dependenciesGraph)
        {
            _dependenciesGraph = dependenciesGraph;
            _permutationGraph = GeneratePermutationGraph(dependenciesGraph);
            new ScheduleOptimizerVisitor(_permutationGraph, dependenciesGraph).Visit();
        }


        private Graph GeneratePermutationGraph(Graph dependenciesGraph)
        {
            var newNodes = dependenciesGraph.AllNodes.Values.Select(p => new Node(p.Id)
            {
                Duration = p.Duration,
                StartTime = p.StartTime
            }).ToDictionary(p => p.Id);

            foreach (var fromNode in newNodes.Values)
            {
                foreach (var toNode in newNodes.Values)
                {
                    //exclude self
                    if (fromNode.Id == toNode.Id)
                        continue;

                    fromNode.Children.Add(toNode);
                }
            }

            //detect root nodes
            var rootNodes = dependenciesGraph.RootNodes.Select(p => newNodes[p.Id]).ToList();

            return new Graph(rootNodes, newNodes);
        }

        /// <summary>
        /// Generate all valid schedules.
        /// </summary>
        public IEnumerable<IReadOnlyList<Node>> Generate() => new ScheduleListGeneratorVisitor(_permutationGraph, _dependenciesGraph).Generate();

        /// <summary>
        /// Returns first earliest schedule.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<Node> GetOptimalSchedule()
        {
            return new OptimalScheduleVisitor(_permutationGraph, _dependenciesGraph).GetOptimalSchedule();
        }

        /// <summary>
        /// Find first earliest schedule. 
        /// </summary>
        private class OptimalScheduleVisitor : ScheduleGeneratorVisitor
        {
            private DateTimeOffset _earliestEndTime = DateTimeOffset.MaxValue;
            private IReadOnlyList<Node>? _selectedPath;

            private readonly Stack<DateTimeOffset> _currentEndTime = new Stack<DateTimeOffset>();

            public OptimalScheduleVisitor(Graph graph, Graph dependenciesGraph) : base(graph, dependenciesGraph)
            {
                _currentEndTime.Push(DateTimeOffset.MinValue);
            }

            public IReadOnlyList<Node> GetOptimalSchedule()
            {
                Visit();

                if (_selectedPath == null)
                    throw new InvalidOperationException("Can not find optimal schedule.");
                return _selectedPath.Reverse().ToArray();
            }

            protected override void BeforeVisit(Node node)
            {
                var endTime = _currentEndTime.Peek();

                if (node.StartTime > endTime)
                    endTime = node.StartTime + node.Duration;
                else
                    endTime += node.Duration;

                _currentEndTime.Push(endTime);
            }

            protected override void PathConstructed()
            {
                var currentTime = _currentEndTime.Peek();
                if (currentTime < _earliestEndTime)
                {
                    _earliestEndTime = currentTime;
                    _selectedPath = CurrentPath.ToArray();
                }
            }

            protected override bool VisitChildren()
            {
                return _currentEndTime.Peek() < _earliestEndTime;
            }

            protected override void AfterVisit(Node node)
            {
                _currentEndTime.Pop();
            }
        }

        /// <summary>
        /// Remove invalid transitions from permutation graph.
        /// </summary>
        private class ScheduleOptimizerVisitor : InDeepVisitor
        {
            private readonly Graph _permutationGraph;

            public ScheduleOptimizerVisitor(Graph permutationGraph, Graph dependenciesGraph) : base(dependenciesGraph)
            {
                _permutationGraph = permutationGraph;
            }

            protected override void BeforeVisit(Node node)
            {
                var permutationNode = _permutationGraph.AllNodes[node.Id];
                permutationNode.Children.ExceptWith(CurrentPath);

                if (CurrentPath.Count > 2)
                {
                    foreach (var transitiveParentNode in CurrentPath.Skip(2))
                    {
                        var permutationTransitiveParentNode = _permutationGraph.AllNodes[transitiveParentNode.Id];
                        permutationTransitiveParentNode.Children.Remove(node);
                    }
                }
            }

        }

        /// <summary>
        /// Find all valid paths in permutation graph according to dependencies graph.
        /// </summary>
        private abstract class ScheduleGeneratorVisitor : InDeepVisitor
        {
            private readonly Graph _dependenciesGraph;

            protected ScheduleGeneratorVisitor(Graph graph, Graph dependenciesGraph) : base(graph)
            {
                _dependenciesGraph = dependenciesGraph;
            }

            protected override bool Skip(Node node)
            {
                if (base.Skip(node))
                    return true;

                var dependencyNode = _dependenciesGraph.AllNodes[node.Id];

                foreach (var parent in dependencyNode.Parents)
                {
                    if (!VisitedNodes.Contains(parent.Id))
                        return true;
                }
                return false;
            }
        }

        private class ScheduleListGeneratorVisitor : ScheduleGeneratorVisitor
        {
            private readonly List<IReadOnlyList<Node>> _permutations = new List<IReadOnlyList<Node>>();

            public ScheduleListGeneratorVisitor(Graph graph, Graph dependenciesGraph) : base(graph, dependenciesGraph)
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

    }
}
