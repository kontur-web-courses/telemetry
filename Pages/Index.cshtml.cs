using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace telemetry.Pages;

public class IndexModel(ILogger<IndexModel> logger, Metrics metrics) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private Metrics _metrics = metrics;

    public void OnGet()
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < new Random().Next(0, 100); i++)
        {
            Thread.Sleep(1);
            _logger.LogInformation("YEP!");
        }

        sw.Stop();
        _metrics.RequestToIndex(sw.Elapsed);
    }
}
