using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using telemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
        .AddMeter(nameof(Metrics))
        .AddPrometheusExporter()
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter());

builder.Services.AddRazorPages();
builder.Services.AddSingleton<Metrics>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
