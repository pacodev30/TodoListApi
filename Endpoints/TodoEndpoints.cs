using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using TodoListApi.Data.Models;

namespace TodoListApi.Endpoints;

public static class TodoEndpoints
{
    public static IServiceCollection AddTodoServices(this IServiceCollection services)
    {
        return services;
    }
    public static RouteGroupBuilder MapTodoEndpoints(this RouteGroupBuilder routeBuilder)
    {
        routeBuilder.MapGet("", GetAll);
        routeBuilder.MapGet("{id:int}", GetById);
        routeBuilder.MapGet("actives", GetActives);
        routeBuilder.MapPost("", Create);
        routeBuilder.MapDelete("{id:int}", Delete);
        routeBuilder.MapPut("{id:int}", Update);

        return routeBuilder;
    }

    private static IResult GetAll()
    {
        return Results.Ok("GetAll");
    }
    private static IResult GetById([FromRoute] int id)
    {
        return Results.Ok($"GetById : {id}");
    }
    private static IResult GetActives() 
    {
        return Results.Ok("GetActive");
    }
    private static IResult Create([FromBody] Todo t)
    {
        return Results.Ok(t);
    }
    private static IResult Delete(int id) 
    {
        return Results.Ok($"Deleted : {id}");
    }
    private static IResult Update(int id)
    {
        return Results.Ok($"Updated {id}") ; 
    }
}
