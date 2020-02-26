using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Scheduler
{
    [DebuggerDisplay("{Id}")]
    public class Node
    {
        public int Id { get; }

        public HashSet<Node> Children { get; }

        public List<Node> Parents { get; } //TODO: remove?

        public DateTimeOffset StartTime { get; }

        public TimeSpan Duration { get; }

        public Node(int id)
        {
            Children = new HashSet<Node>(new NodeEqualityComparer());
            Parents = new List<Node>();
            Id = id;
        }

        private class NodeEqualityComparer : IEqualityComparer<Node>
        {
            public bool Equals(Node x, Node y)
            {
                if (x == y)
                    return true;

                if (x == null || y == null)
                    return false;

                return x.Id == y.Id;
            }

            public int GetHashCode(Node obj)
            {
                return obj.Id;
            }
        }
    }
}
