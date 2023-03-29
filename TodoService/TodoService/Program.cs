using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Web;
using RPC;
using RPC.Shared;
using TodoService.BLL.Services;
using TodoService.BLL.Shared;
using TodoService.DAL.Data;
using TodoService.DAL.Data.Models;
using TodoService.DAL.Data.Repository;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var env = builder.Environment.EnvironmentName;
    if (env == "Development")
        builder.Configuration.AddUserSecrets("TodoService-Dev");
    else
        builder.Configuration.AddUserSecrets("TodoService-noDev");

    builder.Services.AddScoped(typeof(IRepository<Todo>), typeof(TodoRepository));    
    
    builder.Services.AddAutoMapper(typeof(BllMappingProfile));
    builder.Services.AddAutoMapper(typeof(RPCMappingProfile));

    builder.WebHost.ConfigureLogging(
        logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        }
    ).UseNLog();

    var connectionString = builder.Configuration.GetConnectionString("TodoDB");
    builder.Services.AddDbContext<TodoContext>(options =>
    {
        options.UseSqlite(connectionString);
    });

    builder.Services.Configure<Tenants>(builder.Configuration.GetSection("Tenants"));

    builder.Services.AddScoped<IBllTodoService, BllTodoService>();    
    // Add services to the container.
    builder.Services.AddGrpc();    

    var app = builder.Build();

    // Configure the HTTP request pipeline.    

    app.MapGrpcService<TodoRPCService>();
    app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}