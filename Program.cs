using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using telemetry;
using OpenTelemetry.Trace;
using OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var serviceName = "AbsoluteTelemetrinema";

builder.Services.AddSingleton<Metrics>();

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        .AddConsoleExporter();
});
builder
    .Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation().AddConsoleExporter())
    .WithMetrics(metrics =>
        metrics
            .AddPrometheusExporter()
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter()
            .AddMeter(nameof(Metrics))
    )
    .UseOtlpExporter();
;

builder.Services.AddRazorPages();

var app = builder.Build();

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
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.MapRazorPages();

app.Run();
