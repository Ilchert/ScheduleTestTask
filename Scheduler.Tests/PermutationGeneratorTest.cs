using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static IEnumerable<object[]> GenerateData()
        {
            yield return new object[] { @"
1 ()
2 ()
3 ()
4 ()",
                @"
1 2 3 4
1 2 4 3
1 3 2 4
1 3 4 2
1 4 2 3
1 4 3 2
2 1 3 4
2 1 4 3
2 3 1 4
2 3 4 1
2 4 1 3
2 4 3 1
3 1 2 4
3 1 4 2
3 2 1 4
3 2 4 1
3 4 1 2
3 4 2 1
4 1 2 3
4 1 3 2
4 2 1 3
4 2 3 1
4 3 1 2
4 3 2 1
" };

            yield return new object[] { @"
1 ()
2 (1)
3 (2)
4 (1)
",

                @"
1 4 2 3
1 2 4 3
1 2 3 4
" };
            
            yield return new object[] { @"
1 ()
2 (1)
3 (1)
4 (2,3)
",

                @"
1 2 3 4
1 3 2 4
" };
            yield return new object[] { @"
1 ()
2 ()
3 (1)
4 (2,3)
5 (4)
6 (4)
7 (5)
8 (5)
9 (6)
",

                @"
1 2 3 4 5 6 7 8 9
1 2 3 4 5 6 7 9 8
1 2 3 4 5 6 8 7 9
1 2 3 4 5 6 8 9 7
1 2 3 4 5 6 9 7 8
1 2 3 4 5 6 9 8 7
1 2 3 4 5 7 6 8 9
1 2 3 4 5 7 6 9 8
1 2 3 4 5 7 8 6 9
1 2 3 4 5 7 9 8 6
1 2 3 4 5 8 6 7 9
1 2 3 4 5 8 6 9 7
1 2 3 4 5 8 7 6 9
1 2 3 4 5 8 9 7 6
1 2 3 4 5 9 7 6 8
1 2 3 4 5 9 7 8 6
1 2 3 4 5 9 8 6 7
1 2 3 4 5 9 8 7 6
1 2 3 4 6 5 7 8 9
1 2 3 4 6 5 7 9 8
1 2 3 4 6 5 8 7 9
1 2 3 4 6 5 8 9 7
1 2 3 4 6 5 9 7 8
1 2 3 4 6 5 9 8 7
1 2 3 4 6 7 8 9 5
1 2 3 4 6 7 9 5 8
1 2 3 4 6 8 7 9 5
1 2 3 4 6 8 9 5 7
1 2 3 4 6 9 5 7 8
1 2 3 4 6 9 5 8 7
1 3 2 4 5 6 7 8 9
1 3 2 4 5 6 7 9 8
1 3 2 4 5 6 8 7 9
1 3 2 4 5 6 8 9 7
1 3 2 4 5 6 9 7 8
1 3 2 4 5 6 9 8 7
1 3 2 4 5 7 6 8 9
1 3 2 4 5 7 6 9 8
1 3 2 4 5 7 8 6 9
1 3 2 4 5 7 9 8 6
1 3 2 4 5 8 6 7 9
1 3 2 4 5 8 6 9 7
1 3 2 4 5 8 7 6 9
1 3 2 4 5 8 9 7 6
1 3 2 4 5 9 7 6 8
1 3 2 4 5 9 7 8 6
1 3 2 4 5 9 8 6 7
1 3 2 4 5 9 8 7 6
1 3 2 4 6 5 7 8 9
1 3 2 4 6 5 7 9 8
1 3 2 4 6 5 8 7 9
1 3 2 4 6 5 8 9 7
1 3 2 4 6 5 9 7 8
1 3 2 4 6 5 9 8 7
1 3 2 4 6 7 8 9 5
1 3 2 4 6 7 9 5 8
1 3 2 4 6 8 7 9 5
1 3 2 4 6 8 9 5 7
1 3 2 4 6 9 5 7 8
1 3 2 4 6 9 5 8 7
2 1 3 4 5 6 7 8 9
2 1 3 4 5 6 7 9 8
2 1 3 4 5 6 8 7 9
2 1 3 4 5 6 8 9 7
2 1 3 4 5 6 9 7 8
2 1 3 4 5 6 9 8 7
2 1 3 4 5 7 6 8 9
2 1 3 4 5 7 6 9 8
2 1 3 4 5 7 8 6 9
2 1 3 4 5 7 9 8 6
2 1 3 4 5 8 6 7 9
2 1 3 4 5 8 6 9 7
2 1 3 4 5 8 7 6 9
2 1 3 4 5 8 9 7 6
2 1 3 4 5 9 7 6 8
2 1 3 4 5 9 7 8 6
2 1 3 4 5 9 8 6 7
2 1 3 4 5 9 8 7 6
2 1 3 4 6 5 7 8 9
2 1 3 4 6 5 7 9 8
2 1 3 4 6 5 8 7 9
2 1 3 4 6 5 8 9 7
2 1 3 4 6 5 9 7 8
2 1 3 4 6 5 9 8 7
2 1 3 4 6 7 8 9 5
2 1 3 4 6 7 9 5 8
2 1 3 4 6 8 7 9 5
2 1 3 4 6 8 9 5 7
2 1 3 4 6 9 5 7 8
2 1 3 4 6 9 5 8 7
" };

        }

        private static readonly char[] Separator = { '\r', '\n' };

        [Theory]
        [MemberData(nameof(GenerateData))]
        public void Generate(string graphData, string expectedData)
        {
            var reader = new DependenciesReader();
            var graph = reader.Read(graphData.Split(Separator, StringSplitOptions.RemoveEmptyEntries));

            var permutationGenerator = new PermutationGenerator(graph);
            var actual = permutationGenerator.Generate().ToArray();

            var expected = expectedData.Split(Separator, StringSplitOptions.RemoveEmptyEntries).OrderBy(p => p).ToArray();

            var formattedActual = new List<string>();
            foreach (var permutation in actual)
            {
                var permutationString = string.Join(" ", permutation.Select(p => p.Id.ToString(CultureInfo.InvariantCulture)));
                _testOutput.WriteLine(permutationString);
                formattedActual.Add(permutationString);
            }

            formattedActual.Sort();

            Assert.Equal(expected, formattedActual);
            Assert.Equal(expected.Length, actual.Length);
        }
    }
}
