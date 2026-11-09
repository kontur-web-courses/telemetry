using System.Diagnostics.Metrics;

namespace telemetry;

public class Metrics
{
    public const string Meter = "Meter";
    private readonly Counter<int> _indexRequestsCount;
    private readonly Histogram<double> _indexRequestsTime;

    public Metrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(Meter);
        _indexRequestsCount = meter.CreateCounter<int>("requests.index.count", "pcs", "Количество запросов");
        _indexRequestsTime = meter.CreateHistogram<double>("requests.index.time", "ms", "Время запроса к index");
    }

    public void RequestToIndex(TimeSpan elapsed)
    {
        _indexRequestsCount.Add(1);
        _indexRequestsTime.Record(elapsed.TotalMilliseconds);
    }
}