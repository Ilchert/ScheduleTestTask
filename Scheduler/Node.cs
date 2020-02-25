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

        public List<Node> Children { get; }

        public List<Node> Parents { get; }

        public DateTimeOffset StartTime { get; }

        public TimeSpan Duration { get; }

        public Node(int id) : this(id, new List<Node>(), new List<Node>())
        {

        }

        public Node(int id, List<Node> children, List<Node> parents)
        {
            Children = children;
            Parents = parents;
            Id = id;
        }
    }
}
