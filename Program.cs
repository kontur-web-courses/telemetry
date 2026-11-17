using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<Metrics>();

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService("telemetry"))
        .AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("telemetry"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddPrometheusExporter()
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter()
        .AddMeter(Metrics.MeterName))
    .UseOtlpExporter();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.Run();