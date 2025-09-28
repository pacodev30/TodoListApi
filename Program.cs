using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using Serilog;
using FluentValidation;
using TodoListApi.Dto;

var builder = WebApplication.CreateBuilder(args);

// -- SERVICES
builder.Services.AddDbContext<TodoApiContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.MapTodoServices();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Serilog
builder.Logging.ClearProviders();
var loggerConfiguration = new LoggerConfiguration()
        //.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console();
var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

// -- APP
var app = builder.Build();

await app.Services
    .CreateScope().ServiceProvider
    .GetRequiredService<TodoApiContext>().Database
    .MigrateAsync();

app.MapGroup("/todos")
    .MapTodoEndpoints();

app.Run();
