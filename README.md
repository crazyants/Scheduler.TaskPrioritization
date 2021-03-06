# Scheduler.TaskPrioritization

Proof of Concept - simple realization of a task prioritization. Long running batch requests must not affect over the urgent real-time requests that should be executed within specific response time (e.g. 100-200 ms). 

All scenarios can be found in __[ApiControllers](https://github.com/flextry/Scheduler.TaskPrioritization/tree/master/Scheduler.TaskPrioritization/ApiControllers)__ directory.

## Scenario 1
* Performance: ``SLOW``
* Processing logic of realtime requests and Batch requests is the same
* No prioritization
* Both use Parallel.For()

## Scenario 2
* Performance: ``SLOW``
* Realtime requests are processing with Highest priority
* Batch requests are processing with Lowest priority
* Both use Parallel.For()

## Scenario 3
* Performance: ``AVERAGE``
* Realtime requests are processing with Highest priority
* Batch requests are processing with Lowest priority
* Only the Realtime requests use Parallel.For()

## Scenario 4
* Performance: ``AVERAGE``
* Similar to ``Scenario 3``
* Realtime requests are processing with Highest priority
* Batch requests are processing with Lowest priority
* Only the Realtime requests use Parallel.For()
* The MaxDegreeOfParallelism in Parallel.For() is set to number of processors on the current machine

## Scenario 5
* Performance: ``AVERAGE``
* The [PriorityScheduler](/Scheduler.TaskPrioritization/Infrastructure/PriorityScheduler.cs) is used to set priority over Task instead via the Thread
* Only the Realtime requests use Parallel.For() within a new started Task
* The MaxDegreeOfParallelism in Parallel.For() is set to number of processors on the current machine
* Max number of concurrent Batch requests at a time is set to ``2``

## Scenario 6
* Performance: ``AVERAGE``
* Using ``Thread`` class with Lowest priority on a single thread for Batch processing
* Only the Realtime requests use Parallel.For() within a new started Task
* The MaxDegreeOfParallelism in Parallel.For() is set to number of processors on the current machine

## Scenario 7
* Performance: ``FASTEST``
* The [PriorityScheduler](/Scheduler.TaskPrioritization/Infrastructure/PriorityScheduler.cs) is used to set priority over Task instead via the Thread
* Only the Realtime requests use Parallel.For()

## Scenario 8
* Performance: ``FASTEST``
* Similar to ``Scenario 7``
* The [PriorityScheduler](/Scheduler.TaskPrioritization/Infrastructure/PriorityScheduler.cs) is used to set priority over Task instead via the Thread
* Only the Realtime requests use Parallel.For()
* The MaxDegreeOfParallelism in Parallel.For() is set to number of processors on the current machine

## Scenario 9
* Performance: ``FASTEST``
* Similar to ``Scenario 8``
* The [PriorityScheduler](/Scheduler.TaskPrioritization/Infrastructure/PriorityScheduler.cs) is used to set priority over Task instead via the Thread
* Only the Realtime requests use Parallel.For() within a new started Task
* The MaxDegreeOfParallelism in Parallel.For() is set to number of processors on the current machine
