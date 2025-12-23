namespace Lab2.Models;

public class PerformanceResults
{
    public string CollectionName { get; set; } = string.Empty;
    public double AddToEndTime { get; set; }
    public double AddToBeginningTime { get; set; }
    public double AddToMiddleTime { get; set; }
    public double RemoveFromBeginningTime { get; set; }
    public double RemoveFromEndTime { get; set; }
    public double RemoveFromMiddleTime { get; set; }
    public double FindByValueTime { get; set; }
    public double? GetByIndexTime { get; set; }
}

