using System.Collections.Generic;

namespace Scheduler
{
    public abstract class InDeepVisitor
    {
        protected Stack<Node> CurrentPath { get; } = new Stack<Node>();

        private HashSet<int> VisitedNodes { get; } = new HashSet<int>();

        private readonly Graph _graph;

        protected InDeepVisitor(Graph graph) => _graph = graph;

        public void Visit()
        {
            Visit(_graph.RootNodes);
        }

        private void Visit(IEnumerable<Node> nodes)
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