using System.Diagnostics.Metrics;

namespace telemetry;

public class Metrics
{
    private readonly Counter<int> indexRequestsCount;
    private readonly Histogram<double> indexRequestsTime;
    private readonly Counter<int> errorCount;
    private readonly Counter<int> slowRequestsCount;

    private int totalRequests;
    private int errorRequests;

    public Metrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(nameof(Metrics));
        
        indexRequestsCount = meter.CreateCounter<int>("requests.index.count", "pcs", "Количество запросов");
        indexRequestsTime = meter.CreateHistogram<double>("requests.index.time", "ms", "Время запроса к index");

        errorCount = meter.CreateCounter<int>(
            "requests.index.errors", 
            "errors", 
            "Количество ошибок при обработке запроса"
        );

        slowRequestsCount = meter.CreateCounter<int>(
            "requests.index.slow", 
            "requests", 
            "Количество медленных запросов (более 50ms)"
        );

        meter.CreateObservableGauge(
            "requests.index.error_rate",
            CalculateErrorRate,
            "ratio",
            "Доля ошибок среди всех запросов (0-1)"
        );
    }

    public void RequestToIndex(TimeSpan elapsed)
    {
        totalRequests++; 
        indexRequestsCount.Add(1);
        indexRequestsTime.Record(elapsed.TotalMilliseconds);

        if (elapsed.TotalMilliseconds > 50)
        {
            slowRequestsCount.Add(1);
        }
    }

    public void RecordError()
    {
        errorRequests++;
        errorCount.Add(1);
    }

    private double CalculateErrorRate()
    {
        if (totalRequests == 0)
            return 0;

        return (double)errorRequests / totalRequests;
    }
}