using System.Net;
using DirectoryService.Api.Extensions;
using DirectoryService.Api.Jobs;
using DirectoryService.Api.Middleware;
using DirectoryService.Api.Middleware.Authentication;
using DirectoryService.Api.Policies;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Config;
using FluentEmail.Liquid;
using FluentScheduler;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using Serilog.Events;

// ReSharper disable CommentTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable global StringLiteralTypo

namespace DirectoryService.Api;

public class Startup
{
    private ILogger<Startup>? _logger;
    private IConfiguration ConfigRoot { get; }

    public Startup(IConfiguration configuration)
    {
        _logger = null;
        ConfigRoot = configuration;
    }

    /// <summary>
    /// Configure Services
    /// </summary>
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()
            .AddJsonOptions(o =>
                o.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy());

        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var baseDir = Path.Combine(Path.GetDirectoryName(assembly.Location)!, "logs/");
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(baseDir + "log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        
        builder.Host.UseSerilog();
        builder.Services.RegisterServices();

        SetupConfiguration();

        var config = ServiceConfigurationContainer.Config;
        
        builder.Services.AddFluentEmail(
                ServiceConfigurationContainer.Config.Smtp.SenderEmail,
                ServiceConfigurationContainer.Config.Smtp.SenderName
            )
            .AddLiquidRenderer()
            .AddSmtpSender(
                ServiceConfigurationContainer.Config.Smtp.Host ?? "localhost",
                ServiceConfigurationContainer.Config.Smtp.Port,
                ServiceConfigurationContainer.Config.Smtp.Username ?? "",
                ServiceConfigurationContainer.Config.Smtp.Password ?? "");

        // Setup Kestrel with http/https options provided in configuration
        builder.WebHost.UseKestrel((hostingContext, options) =>
        {
            options.Listen(IPAddress.Any, config.Server.HttpPort);
            if (config.Server.UseHttps)
            {
                options.Listen(IPAddress.Any, config.Server.HttpsPort, listenOptions =>
                {
                    if (config.Server.HttpsCertPassword is not null && config.Server.HttpsCertPassword != "")
                    {
                        if (config.Server.HttpsCertFile is null || !File.Exists(config.Server.HttpsCertFile))
                            throw new Exception("Https Certificate not found");

                        listenOptions.UseHttps(config.Server.HttpsCertFile!,
                            config.Server.HttpsCertPassword);
                    }
                    else
                    {
                        if (config.Server.HttpsCertFile is null || !File.Exists(config.Server.HttpsCertFile))
                            throw new Exception("Https Certificate not found");
                        listenOptions.UseHttps(config.Server.HttpsCertFile!);
                    }
                });
            }
        });
    }

    /// <summary>
    /// Configure Application
    /// </summary>
    public void Configure(WebApplication app)
    {
        _logger = app.Services.GetRequiredService<ILogger<Startup>>();

        ConfigureForwardHeaders(app);

        app.UseMiddleware<AuthMiddleware>();
        app.UseStatusCodeExceptionHandler();
        app.UseApiExceptionHandler();
        
        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
        
        app.MapControllers();
        
        DbContext.Configure();
        
        _logger.LogInformation("---------- Server Starting ----------");
        if (!DatabaseMigrator.MigrateDatabase(_logger))
            return;

        // If Development then truncate data and seed dev data
        if (app.Environment.IsDevelopment())
        {
            var dbContext = app.Services.GetRequiredService<DbContext>();
            if (Environment.GetEnvironmentVariable("ODS_SKIP_DEV_WARNING") == null ||
                bool.Parse(Environment.GetEnvironmentVariable("ODS_SKIP_DEV_WARNING")!) == false)
            {
                _logger.LogWarning(
                    "Development Mode: Truncating all data in 5 seconds. Terminate application NOW if using production data.");
                Thread.Sleep(5000);
            }

            // dbContext.RunScript("truncateAll.sql");
            // _logger.LogInformation("Database truncated. Seeding dev data");
            // dbContext.RunScript("devSeed.sql");
        }
        
        // Setup scheduled jobs
        var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
        JobManager.Initialize(new JobRegistry(serviceScopeFactory));
        
        _logger.LogInformation("Http Port: {port}", ServiceConfigurationContainer.Config.Server.HttpPort);
        if(ServiceConfigurationContainer.Config.Server.UseHttps)
            _logger.LogInformation("Https Port: {port}", ServiceConfigurationContainer.Config.Server.HttpsPort);
        _logger.LogInformation("Running Server");
        app.Run();
    }

    /// <summary>
    /// Separating appsettings.json from service configuration to prevent
    /// mishaps that could break the way Kestrel operates
    /// </summary>
    private static void SetupConfiguration()
    {
        if (!File.Exists("./config/serviceConfig.json"))
        {
            Log.Error("Configuration file was not found. If using docker, be sure to mount the config directory containing the server configuration to '/app/config' i.e: docker run -v \"$(pwd)/DirectoryService.Api/config\":/app/config ...");
            Environment.Exit(2);
        }

        using var r = new StreamReader("./config/serviceConfig.json");
        var json = r.ReadToEnd();
        
        ServiceConfigurationContainer.Config = JsonConvert.DeserializeObject<ServiceConfiguration>(json)!;
    }

    /// <summary>
    /// If behind reverse proxies (such as Cloudflare) getting requester IP
    /// will not be simple, so providing known proxy IP addresses will help accuracy
    /// </summary>
    /// <param name="app"></param>
    private void ConfigureForwardHeaders(IApplicationBuilder app)
    {
        
        if (ServiceConfigurationContainer.Config.Server.KnownProxies!.Count == 0)
            return;

        _logger!.LogInformation("Configuring Forwarded Headers");
        var opts = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All,
            RequireHeaderSymmetry = false,
            ForwardLimit = null,
        };
        foreach (var proxy in ServiceConfigurationContainer.Config.Server.KnownProxies)
        {
            opts.KnownProxies.Add(IPAddress.Parse(proxy));
        }

        app.UseForwardedHeaders(opts);
    }
}