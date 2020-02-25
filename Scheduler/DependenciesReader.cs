using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scheduler
{
    public class DependenciesReader
    {
        public Graph Read(IEnumerable<string> data)
        {
            var nodeIndex = new Dictionary<int, Node>();

            foreach (var line in data)
            {
                var lineData = line.Split(new[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries); //TODO: to span.
                if (lineData.Length == 0)
                    throw new FormatException();

                if (!int.TryParse(lineData[0], out var nodeId))
                    throw new FormatException();

                if (!nodeIndex.TryGetValue(nodeId, out var currentNode))
                    currentNode = nodeIndex[nodeId] = new Node(nodeId);

                if (lineData.Length == 1)
                    continue;

                if (lineData.Length != 2)
                    throw new FormatException();

                var relatedIdsData = lineData[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var parentIdData in relatedIdsData)
                {
                    if (!int.TryParse(parentIdData, out var parentId))
                        throw new FormatException();

                    if (!nodeIndex.TryGetValue(parentId, out var parentNode))
                        parentNode = nodeIndex[parentId] = new Node(parentId);

                    parentNode.Children.Add(currentNode);
                    currentNode.Parents.Add(parentNode);
                }
            }

            var rootNodes = nodeIndex.Values.Where(p => p.Parents.Count == 0).ToList();
            return new Graph(rootNodes, nodeIndex);
        }
    }
}
