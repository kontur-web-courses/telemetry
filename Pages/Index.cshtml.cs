using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenTelemetry.Metrics;
using System.Diagnostics;

namespace telemetry.Pages;

public class IndexModel : PageModel
{
    private readonly Metrics _metrics;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(Metrics metrics, ILogger<IndexModel> logger)
    {
        _metrics = metrics;
        _logger = logger;
    }

    public void OnGet()
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < new Random().Next(0, 100); i++)
        {
            Console.Write("1");
        }
        sw.Stop();
        _metrics.RequestToIndex(sw.Elapsed);

        _logger.LogInformation($"Время выполнения {sw.ElapsedMilliseconds}ms");
    }
}
