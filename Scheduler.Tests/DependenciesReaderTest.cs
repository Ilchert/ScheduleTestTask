using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Scheduler.Tests
{
    public class DependenciesReaderTest
    {
        [Fact]
        public void Read()
        {
            var reader = new DependenciesReader();
            var data = new[]
            {
                "1 ()",
                "2 ()",
                "3 (1)",
                "4 (2,3)",
            };

            var dependencies = reader.Read(data).RootNodes;

            Assert.NotNull(dependencies);
            Assert.NotEmpty(dependencies);
            var orderedDependencies = dependencies.OrderBy(p => p.Id).ToArray();

            Assert.Collection(orderedDependencies,
                p => Assert.Equal(1, p.Id),
                p => Assert.Equal(2, p.Id));

            Assert.Empty(orderedDependencies[0].Parents);
            Assert.Collection(orderedDependencies[0].Children, p => Assert.Equal(3, p.Id));

            Assert.Empty(orderedDependencies[1].Parents);
            Assert.Collection(orderedDependencies[1].Children, p => Assert.Equal(4, p.Id));

            var fourNode = orderedDependencies[1].Children.First();
            Assert.Empty(fourNode.Children);
            Assert.Collection(fourNode.Parents.OrderBy(p => p.Id),
                p => Assert.Equal(2, p.Id),
                p => Assert.Equal(3, p.Id));
        }
    }
}
