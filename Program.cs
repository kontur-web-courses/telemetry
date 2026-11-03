using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using telemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService("ABOBA_3_SPIDOMETER"))
        .AddConsoleExporter();
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("ABOBA_3_SPIDOMETER"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddPrometheusExporter()
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter()
        .AddMeter(nameof(Metrics)))
    .UseOtlpExporter();

builder.Services.AddMetrics();
builder.Services.AddSingleton<Metrics>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
