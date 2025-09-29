using Microsoft.AspNetCore.Mvc;
using TodoListApi.Services;
using FluentValidation;
using TodoListApi.Data.Models;

namespace TodoListApi.Endpoints;

public static class TodoEndpoints
{
    public static IServiceCollection MapTodoServices(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, EFCoreTodoService>();
        return services;
    }
    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder routeBuilder)
    {
        // GET todos
        routeBuilder.MapGet("", GetAll)
            .Produces(404)
            .Produces(200)
            .WithName("GetAll")
            .WithTags("todoManagement");
        // GET todos/1
        routeBuilder.MapGet("{id:int}", GetById)
            .Produces(404)
            .Produces(200)
            .WithName("GetById")
            .WithTags("todoManagement");
        // GET todos/actives
        routeBuilder.MapGet("actives", GetActives)
            .Produces(404)
            .Produces(200)
            .WithName("GetActives")
            .WithTags("todoManagement");
        // POST todos
        routeBuilder.MapPost("", Add)
            .Produces(400)
            .Produces(202)
            .WithName("Create")
            .WithTags("todoManagement");
        // DELETE todos/1
        routeBuilder.MapDelete("{id:int}", Delete)
            .Produces(404)
            .Produces(204)
            .WithName("Delete")
            .WithTags("todoManagement");
        // PUT todos/1
        routeBuilder.MapPut("{id:int}", Update)
            .Produces(404)
            .Produces(201)
            .WithName("Update")
            .WithTags("todoManagement");

        return routeBuilder;
    }
    // GetAll
    private async static Task<IResult> GetAll(
        [FromServices] ITodoService service,
        [FromServices] ILogger<Program> logger,
        [FromServices] AuthService authService,
        HttpContext httpContext)
    {
        var userId = await authService.GetIdUserFromToken(httpContext);
        if (!userId.HasValue) return Results.Unauthorized();

        var todos = await service.GetAll(userId.Value);

        logger.LogInformation($"Get {todos.Count} todos");
        return Results.Ok(todos);
    }
    // GetById
    private static async Task<IResult> GetById(
        [FromRoute] int id,
        [FromServices] ITodoService service,
        [FromServices] ILogger<Program> logger,
        [FromServices] AuthService authService,
        HttpContext httpContext)
    {
        var userId = await authService.GetIdUserFromToken(httpContext);
        if (!userId.HasValue) return Results.Unauthorized();

        var todo = await service.GetById(id, userId.Value);

        if(todo is null) return Results.NotFound();
        logger.LogInformation($"Get Toto {todo.Id}");
        return Results.Ok(todo);
    }
    // GetActives
    private static async Task<IResult> GetActives(
        [FromServices] ITodoService service,
        [FromServices] ILogger<Program> logger,
        [FromServices] AuthService authService,
        HttpContext httpContext) 
    {
        var userId = await authService.GetIdUserFromToken(httpContext);
        if (!userId.HasValue) return Results.Unauthorized();

        var todos = await service.GetActives(userId.Value);
        logger.LogInformation($"Get {todos.Count} todos actives");
        return Results.Ok(todos);
    }
    // Create
    private static async Task<IResult> Add(
        [FromBody] TodoInputModel newTodo,
        [FromServices] ITodoService service,
        [FromServices] IValidator<TodoInputModel> validator,
        [FromServices] ILogger<Program> logger,
        [FromServices] LinkGenerator linkGenerator,
        [FromServices] AuthService authService,
        HttpContext httpContext)
    {
        var result = validator.Validate(newTodo);
        if(!result.IsValid)
        {
            var error = result.Errors.Select(e => new 
            {
                e.ErrorMessage,
                e.PropertyName
            });
            return Results.BadRequest(error);
        }
        var userId = await authService.GetIdUserFromToken(httpContext);
        if (!userId.HasValue) return Results.Unauthorized();

        var createdValue = await service.Add(newTodo, userId.Value);
        logger.LogInformation($"Created {createdValue.Title}");
        var link = linkGenerator.GetUriByName(httpContext, "GetById", new {id = createdValue.Id});
        return Results.Created(link, createdValue);
    }
    // Delete
    private async static Task<IResult> Delete(
        [FromRoute] int id,
        [FromServices] ITodoService service,
        [FromServices] ILogger<Program> logger,
        [FromServices] AuthService authService,
        HttpContext httpContext) 
    {
        var userId = await authService.GetIdUserFromToken(httpContext);
        if (!userId.HasValue) return Results.Unauthorized();

        var todoToDelete = await service.Delete(id, userId.Value);

        if(!todoToDelete) return Results.NotFound();
        logger.LogInformation($"Delete {id}");
        return Results.NoContent();
    }
    // Update
    private async static Task<IResult> Update(
        [FromRoute] int id,
        [FromBody] TodoInputModel todo,
        [FromServices] ITodoService service,
        [FromServices] ILogger<Program> logger,
        [FromServices] LinkGenerator linkGenerator,
        [FromServices] AuthService authService,
        HttpContext httpContext)
    {
        var userId = await authService.GetIdUserFromToken(httpContext);
        if (!userId.HasValue) return Results.Unauthorized();

        var dbTodo = await service.Update(id, userId.Value, todo);
        if(!dbTodo) return Results.NotFound();
        var link = linkGenerator.GetUriByName(httpContext, "GetById", id);
        logger.LogInformation($"{todo.Title} updated | {link}");
        return Results.Created(link, dbTodo);
    }
}
