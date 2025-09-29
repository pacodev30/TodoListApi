using Microsoft.AspNetCore.Mvc;
using System.Text;
using TodoListApi.Data;
using TodoListApi.Data.Models;

namespace TodoListApi.Endpoints
{
    public static class  UserEndpoints
    {
        private const string TokenChar = "ABCDEFGHIJKLMNNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static IServiceCollection MapUserServices(this IServiceCollection services)
        {
            return services;
        }

        public static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder routeBuilder) 
        {
            routeBuilder.MapPost("", Add);
            return routeBuilder;
        }

        private static async Task<IResult> Add(
            [FromBody] UserInputModel userModel,
            [FromServices] TodoApiContext context)
        {
            var sb = new StringBuilder(16);
            for (int i = 0; i < 16; i++)
            {
                sb.Append(TokenChar[Random.Shared.Next(0, TokenChar.Length)]);
            }
            var user = new User 
            {
                Name = userModel.Name,
                Token = sb.ToString(),
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Results.Ok(new UserOutputModel(user.Id, user.Name, user.Token));
        }
    }
}
