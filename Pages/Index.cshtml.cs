using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace telemetry.Pages;

public class IndexModel(ILogger<IndexModel> logger, Metrics metrics) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;

    public void OnGet()
    {
        var sw = Stopwatch.StartNew();
        var random = new Random();
        
        try
        {
            for (var i = 0; i < random.Next(0, 100); i++)
            {
                if (random.Next(100) < 5)
                {
                    throw new Exception("Simulated processing error");
                }
                
                Console.Write("1");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке запроса");
            metrics.RecordError(); 
        }
        finally
        {
            sw.Stop();
            
            metrics.RequestToIndex(sw.Elapsed);
        }
    }
}