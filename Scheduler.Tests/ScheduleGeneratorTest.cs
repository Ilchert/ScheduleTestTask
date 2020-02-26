using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Scheduler.Tests
{
    public class ScheduleGeneratorTest
    {
        private readonly ITestOutputHelper _testOutput;

        public ScheduleGeneratorTest(ITestOutputHelper testOutput)
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

            var scheduleGenerator = new ScheduleGenerator(graph);
            var actual = scheduleGenerator.Generate().ToArray();

            var expected = expectedData.Split(Separator, StringSplitOptions.RemoveEmptyEntries).OrderBy(p => p).ToArray();

            var formattedActual = new List<string>();
            foreach (var schedule in actual)
            {
                var formattedString = string.Join(" ", schedule.Select(p => p.Id.ToString(CultureInfo.InvariantCulture)));
                _testOutput.WriteLine(formattedString);
                formattedActual.Add(formattedString);
            }

            formattedActual.Sort();

            Assert.Equal(expected, formattedActual);
            Assert.Equal(expected.Length, actual.Length);
        }

        public static IEnumerable<object[]> GetOptimalScheduleData()
        {
            yield return new object[] { @"
1 ()
2 (1)
3 (2)
4 (1)
",
                @"
1 2020-06-22T12:00:00 01:00:00
2 2020-06-23T12:00:00 00:30:00
3 2020-06-22T12:00:00 00:10:00
4 2020-06-23T10:00:00 00:05:00
",

                @"1 4 2 3" };

        }

        [Theory]
        [MemberData(nameof(GetOptimalScheduleData))]
        public void GetOptimalSchedule(string graphData, string timeInformation, string expected)
        {
            var reader = new DependenciesReader();
            var graph = reader.Read(graphData.Split(Separator, StringSplitOptions.RemoveEmptyEntries));
            var additionalInformationReader = new AdditionalInformationReader();
            additionalInformationReader.FillData(timeInformation.Split(Separator, StringSplitOptions.RemoveEmptyEntries), graph);

            var scheduleGenerator = new ScheduleGenerator(graph);


            var actual = scheduleGenerator.GetOptimalSchedule().ToArray();

            var formattedActual = string.Join(" ", actual.Select(p => p.Id.ToString(CultureInfo.InvariantCulture)));
            _testOutput.WriteLine(formattedActual);

            Assert.Equal(expected, formattedActual);
        }
    }
}
