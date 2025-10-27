using System.Diagnostics.Metrics;

namespace telemetry;

public class Metrics
{
    private readonly Counter<int> indexRequestsCount;
    private readonly Counter<double> indexRequestsPerSec;
    private readonly Histogram<double> indexRequestsTime;

    public Metrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(nameof(Metrics));
        indexRequestsCount = meter.CreateCounter<int>("requests.index.count", "pcs", "Количество запросов");
        indexRequestsPerSec = meter.CreateCounter<double>("requests.index.per.sec", "rps", "Количество запросов в секунду");
        indexRequestsTime = meter.CreateHistogram<double>("requests.index.time", "ms", "Время запроса к index");
    }

    public void RequestToIndex(TimeSpan elapsed)
    {
        indexRequestsCount.Add(1);
        indexRequestsTime.Record(elapsed.TotalMilliseconds);
        indexRequestsPerSec.Add((double) 1 / elapsed.TotalMilliseconds);
    }
}