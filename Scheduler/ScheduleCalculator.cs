using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class ScheduleCalculator
    {
        public static IReadOnlyList<IReadOnlyList<Node>> GetAllSchedules(IReadOnlyList<Node> startNodes)
        {
            return new AllSchedulesVisitor(startNodes).GetAllSchedules();
        }

        private class AllSchedulesVisitor
        {
            private readonly IReadOnlyList<Node> _startNodes;
            private List<List<Node>> _allSchedules;
            private List<Node>? _currentPath;

            public AllSchedulesVisitor(IReadOnlyList<Node> startNodes)
            {
                _startNodes = startNodes;
                _allSchedules = new List<List<Node>>();
            }

            public IReadOnlyList<IReadOnlyList<Node>> GetAllSchedules()
            {

                return Array.Empty<IReadOnlyList<Node>>();
            }

            private void VisitNode(Node node)
            {
                _currentPath.Add(node);
                foreach (var nextNode in node.Children)
                {
                    VisitNode(node);
                }
            }

        }
    }
}
