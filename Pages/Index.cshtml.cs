using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace telemetry.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Metrics metrics;

    public IndexModel(ILogger<IndexModel> logger, Metrics metrics)
    {
        _logger = logger;
        this.metrics = metrics;
    }

    public void OnGet()
    {
        var sw = Stopwatch.StartNew();
        for (var i = 0; i < new Random().Next(0, 100); i++)
        {
            Console.Write("1");
        }
        sw.Stop();
        metrics.RequestToIndex(sw.Elapsed);
    }
}
