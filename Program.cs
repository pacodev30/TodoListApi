using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using Serilog;
using FluentValidation;
using TodoListApi.Endpoints;
using TodoListApi.Services;

var builder = WebApplication.CreateBuilder(args);

// -- SERVICES
builder.Services.AddDbContext<TodoApiContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("SqlServer")));
builder.Services.MapTodoServices();
builder.Services.MapUserServices();
builder.Services.AddScoped<AuthService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Serilog
builder.Logging.ClearProviders();
var loggerConfiguration = new LoggerConfiguration()
        //.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console();
var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

// -- APP
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services
    .CreateScope().ServiceProvider
    .GetRequiredService<TodoApiContext>().Database
    .MigrateAsync();

app.MapGroup("/todos")
    .MapTodoEndpoints();
app.MapGroup("/users")
    .MapUserEndpoints();

app.Run();
