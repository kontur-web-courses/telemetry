using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace telemetry.Pages;

public class IndexModel : PageModel
{
    private readonly Metrics _metrics;

    public IndexModel(Metrics metrics)
    {
        _metrics = metrics;
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
    }
}
