namespace Ordering.Worker;

public class WorkerOptions
{
    public string? ConnectionString { get; set; }
    public string? EventBusConnection { get; set; }
    public string? SubscriptionClientName { get; set; }
    public int CheckUpdateTime { get; set; }
}
