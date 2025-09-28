using Microsoft.AspNetCore.Mvc;
using TodoListApi.Data.Models;
using TodoListApi.Services;
using FluentValidation;
using TodoListApi.Dto;

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
        routeBuilder.MapGet("", GetAll);
        routeBuilder.MapGet("{id:int}", GetById)
            .WithName("GetById");
        routeBuilder.MapGet("actives", GetActives);
        routeBuilder.MapPost("", Create);
        routeBuilder.MapDelete("{id:int}", Delete);
        routeBuilder.MapPut("{id:int}", Update);

        return routeBuilder;
    }
    // GetAll
    private async static Task<IResult> GetAll(
        [FromServices] ITodoService service)
    {
        var todos = await service.GetAll();
        return Results.Ok(todos);
    }
    // GetById
    private static async Task<IResult> GetById(
        [FromRoute] int id,
        [FromServices] ITodoService service)
    {
        var todo = await service.GetById(id);
        if(todo is null) return Results.NotFound();
        return Results.Ok(todo);
    }
    // GetActives
    private static async Task<IResult> GetActives(
        [FromServices] ITodoService service) 
    {
        var todos = await service.GetActives();
        return Results.Ok(todos);
    }
    // Create
    private static async Task<IResult> Create(
        [FromBody] TodoInputModel newTodo,
        [FromServices] ITodoService Service,
        [FromServices] IValidator<TodoInputModel> validator,
        [FromServices] LinkGenerator linkGenerator,
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
        var createdValue = await Service.Create(newTodo);
        var link = linkGenerator.GetUriByName(httpContext, "GetById", new {id = createdValue.Id});
        return Results.Created(link, createdValue);
    }
    // Delete
    private async static Task<IResult> Delete(
        [FromRoute] int id,
        [FromServices] ITodoService service) 
    {
        var todoToDelete = await service.Delete(id);
        if(!todoToDelete) return Results.NotFound();
        return Results.NoContent();
    }
    // Update
    private async static Task<IResult> Update(
        [FromRoute] int id,
        [FromBody] TodoInputModel todo,
        [FromServices] ITodoService service)
    {
        var dbTodo = await service.Update(id, todo);
        if(!dbTodo) return Results.NotFound();
        return Results.Ok(dbTodo);
    }
}
