using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace telemetry.Pages;

public class IndexModel(ILogger<IndexModel> logger, Metrics metrics) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly Metrics metrics = metrics;

    public void OnGet()
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < new Random().Next(0, 100); i++)
        {
            Console.Write("1");
        }
        sw.Stop();
        metrics.RequestToIndex(sw.Elapsed);
        _logger.LogInformation("Add metrics 1");
    }
}
