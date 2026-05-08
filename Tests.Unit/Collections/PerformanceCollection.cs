namespace Tests.Unit.Collections;

/// <summary>
/// Collection definition for performance tests. Tests in this collection
/// run sequentially and never in parallel with each other, ensuring
/// stable timing measurements.
/// </summary>
[CollectionDefinition(TestCollections.Performance, DisableParallelization = true)]
public class PerformanceCollection;

