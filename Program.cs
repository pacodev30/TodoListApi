using Microsoft.AspNetCore.Mvc;
using Serilog;
using TodoListApi;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddSingleton<TodoService>();
builder.Services.AddOpenApiDocument();

// Logger : Serilog.AspNetCore
builder.Logging.ClearProviders();
var loggerConfiguration = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day);
var logger = loggerConfiguration.CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

// Get All
app.MapGet("todos", (
    [FromServices] TodoService service, 
    [FromServices] ILogger<TodoService> logger) =>
{
    logger.LogInformation("Getting all todos");
    return Results.Ok(service.GetAll());
});

// Get active
app.MapGet("todos/active", (
    [FromServices] TodoService service,
    [FromServices] ILogger<TodoService> logger) =>
{
    logger.LogInformation("Getting active todos");
    return Results.Ok(service.GetAll().Where(t => t.EndDate == null));
});

// Get by id
app.MapGet("todos/{id:int}", (
    [FromRoute] int id,
    [FromServices] TodoService service,
    [FromServices] ILogger<TodoService> logger) =>
{
    logger.LogInformation("Getting todo by id: {Id}", id);
    var todo = service.GetById(id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

// Create
app.MapPost("todos", (
    [FromBody] string title,
    [FromServices] TodoService service,
    [FromServices] ILogger<TodoService> logger) =>
{
    logger.LogInformation("Creating a new todo with title: {Title}", title);
    var newToto = service.Add(title);
    return Results.Ok(newToto);
});

// Delete
app.MapDelete("todos/{id:int}", (
    [FromRoute] int id,
    [FromServices] TodoService service,
    [FromServices] ILogger<TodoService> logger) =>
{
    logger.LogInformation("Deleting todo with id: {Id}", id);
    var todoDeleted = service.Delete(id);
    return todoDeleted ? Results.NoContent() : Results.NotFound();
});

// Update
app.MapPut("todos/{id:int}", (
    [FromRoute] int id,
    [FromBody] Todo item,
    [FromServices] TodoService service,
    [FromServices] ILogger<TodoService> logger) =>
{
    logger.LogInformation("Updating todo with id: {Id}", id);
    service.Update(id, item);
    return Results.NoContent();
});

app.Run();
