using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Scheduler.Tests
{
    public class AdditionalInformationReaderTest
    {
        [Fact]
        public void Read()
        {
            var data = new[]
            {
                "1 2020-06-22T12:00:00 01:00:00",
                "2 2021-06-22T00:00:00 00:01:00",
            };

            var firstNode = new Node(1);
            var secondNode = new Node(2);

            var nodes = new Dictionary<int, Node>()
            {
                { 1, firstNode},
                { 2, secondNode},
            };
            var graph = new Graph(nodes.Values.ToList(), nodes);

            var reader = new AdditionalInformationReader();
            reader.FillData(data, graph);

            Assert.Equal(new DateTimeOffset(2020, 06, 22, 12, 0, 0, TimeSpan.Zero), firstNode.StartTime);
            Assert.Equal(TimeSpan.FromHours(1), firstNode.Duration);

            Assert.Equal(new DateTimeOffset(2021, 06, 22, 0, 0, 0, TimeSpan.Zero), secondNode.StartTime);
            Assert.Equal(TimeSpan.FromMinutes(1), secondNode.Duration);
        }
    }
}
