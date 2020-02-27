This project generates all possible schedules for specific tasks and his relations and finds an optimal schedule.
The main class is ScheduleGenerator, other classes are helper classes for parsing, graph representation, etc and not important in this case.

Generate() - Generates all possible schedules according to provided dependencies graph.
Shedule generation is the NP full task with n! complexity, so we perform some optimizations. Let's start with an example: 
graph:
1 ()
2 ()
3 (1,2)
4 (2)
First of all, generate a possible permutation graph (GeneratePermutationGraph).
1 (2,3,4)
2 (1,3,4)
3 (1, 2,4)
4 (1, 2,3)
With 2 roots: 1,2.
Next, optimize possible permutations according to current dependencies (ScheduleOptimizerVisitor) using the following roles:
child node can not have relation its parents (can not travel 3->1, 3-> 2, 4->2)
  a parent can not travel to grandchildren (can not travel from 1 to 3'th children).
So we optimized graph to
1 (2,3,4)
2 (1,3,4)
3 (4)
4(1,3)
Now we just traves graph and any path with length 4 (node count) can be the schedule (ScheduleGeneratorVisitor), but we have to check node availability to traverse (check that all its parents already visited).
Generated shedules:
1 2 3 4 - valid
1 2 4 3 - valid
2 1 3 4 - valid
2 1 4 3 - valid
2 3 4 1 - invalid (can't visit 3 before 1)
2 4 1 3 - valid

GetOptimalSchedule - get the first schedule with the earliest end time. (OptimalScheduleVisitor)
To find such a shedule, calculate the end time of every node in the path, choose the path with the earliest last node time.
In this case, we can optimize calculation using tricks:
incrementally calculate time during building the schedule, so we won't calculate them for similar schedules
stop processing nodes, if its end time more than the shortest time, which we already founded.