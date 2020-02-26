using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Scheduler
{
    public class AdditionalInformationReader
    {
        public void FillData(IEnumerable<string> data, Graph graph)
        {
            foreach (var line in data)
            {
                var lineData = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (lineData.Length != 3)
                    throw new FormatException();

                if (!int.TryParse(lineData[0], out var nodeId))
                    throw new FormatException();

                if (!DateTimeOffset.TryParseExact(lineData[1], "s", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var startTime))
                    throw new FormatException();

                if (!TimeSpan.TryParse(lineData[2], CultureInfo.InvariantCulture, out var duration))
                    throw new FormatException();

                var node = graph.AllNodes[nodeId];
                node.StartTime = startTime;
                node.Duration = duration;
            }
        }
    }
}
