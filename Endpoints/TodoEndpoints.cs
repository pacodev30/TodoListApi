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
            .WithTags("todoManagement")
            .WithName("GetAll")
            .Produces<TodoOutputModel[]>(200, "application/json")
            .Produces(401);
        // GET todos/1
        routeBuilder.MapGet("{id:int}", GetById)
            .WithTags("todoManagement")
            .WithName("GetById")
            .Produces<TodoOutputModel>(200, "application/json")
            .Produces(401);
        // GET todos/actives
        routeBuilder.MapGet("actives", GetActives)
            .WithTags("todoManagement")
            .WithName("GetActives")
            .Produces<TodoOutputModel[]>(200, "application/json")
            .Produces(401);
        // POST todos
        routeBuilder.MapPost("", Add)
            .WithTags("todoManagement")
            .WithName("Create")
            .Produces(201)
            .Produces(400)
            .Produces(401);
        // DELETE todos/1
        routeBuilder.MapDelete("{id:int}", Delete)
            .WithTags("todoManagement")
            .WithName("Delete")
            .Produces(204)
            .Produces(401);
        // PUT todos/1
        routeBuilder.MapPut("{id:int}", Update)
            .WithTags("todoManagement")
            .WithName("Update")
            .Produces(204)
            .Produces(400)
            .Produces(401);

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
    // Add
    private static async Task<IResult> Add(
        [FromBody] TodoInputModel newTodo,
        [FromServices] ITodoService service,
        [FromServices] ILogger<Program> logger,
        [FromServices] LinkGenerator linkGenerator,
        [FromServices] AuthService authService,
        [FromServices] IValidator<TodoInputModel> validator,
        HttpContext httpContext)
    {
        var validationResult = validator.Validate(newTodo);
        if(!validationResult.IsValid)
        {
            var error = validationResult.Errors.Select(e => new 
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
        [FromServices] IValidator<TodoInputModel> validator,
        HttpContext httpContext)
    {
        var validationResult = validator.Validate(todo);
        if (!validationResult.IsValid) return Results.BadRequest(validationResult.Errors);

        if (id < 0) return Results.BadRequest();

        var userId = await authService.GetIdUserFromToken(httpContext);
        if (!userId.HasValue) return Results.Unauthorized();

        var dbTodo = await service.Update(id, userId.Value, todo);
        if(!dbTodo) return Results.NotFound();
        var link = linkGenerator.GetUriByName(httpContext, "GetById", id);
        logger.LogInformation($"{todo.Title} updated | {link}");
        return Results.Created(link, dbTodo);
    }
}
