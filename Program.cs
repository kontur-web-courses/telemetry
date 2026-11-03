using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using telemetry;
using OpenTelemetry;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);
var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? "telemetry";

builder.Services.AddRazorPages();
builder.Services.AddSingleton<Metrics>();

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName))
        .AddConsoleExporter();
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService(serviceName))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter())
    .WithMetrics(metrics => metrics
        .AddPrometheusExporter()
        .AddAspNetCoreInstrumentation()
        .AddConsoleExporter()
        .AddMeter(nameof(Metrics)))
        .UseOtlpExporter();



// Add services to the container.


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
