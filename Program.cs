using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService("TelemetryExample"))
        .AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("TelemetryExample"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddPrometheusExporter()
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter()
        .AddMeter(nameof(Metrics))
    )
    .UseOtlpExporter();

var app = builder.Build();
app.UseOpenTelemetryPrometheusScrapingEndpoint();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();