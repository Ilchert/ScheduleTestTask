using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Scheduler.Tests
{
    public class PermutationGeneratorTest
    {
        private readonly ITestOutputHelper _testOutput;

        public PermutationGeneratorTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public void Generate_NoDependencies()
        {
            var reader = new DependenciesReader();
            var data = new[]
            {
                "1 ()",
                "2 ()",
                "3 ()",
                "4 ()"
            };

            var graph = reader.Read(data);

            var permutationGenerator = new PermutationGenerator(graph);
            var permutations = permutationGenerator.Generate();

            foreach (var permutation in permutations)
            {
                _testOutput.WriteLine(string.Join(" ", permutation.Select(p => p.Id)));
            }

            Assert.Equal(24, permutations.Count());
        }

        [Fact]
        public void Generate_BasicDependencies()
        {
            var reader = new DependenciesReader();
            var data = new[]
            {
                "1 ()",
                "2 (1)",
                "3 (2)",
                "4 (1)"
            };

            var graph = reader.Read(data);

            var permutationGenerator = new PermutationGenerator(graph);
            var permutations = permutationGenerator.Generate();

            foreach (var permutation in permutations)
            {
                _testOutput.WriteLine(string.Join(" ", permutation.Select(p => p.Id)));
            }

            Assert.Equal(3, permutations.Count());
            
        }
    }
}
