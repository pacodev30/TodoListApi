using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using TodoListApi.Data;
using TodoListApi.Data.Models;
using TodoListApi.Validation;

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
            routeBuilder.MapPost("", Add)
                .WithTags("UserManagement")
                .Produces(400)
                .Produces<UserOutputModel>(200, "application/json");
            return routeBuilder;
        }

        private static async Task<IResult> Add(
            [FromBody] UserInputModel inputModel,
            [FromServices] TodoApiContext context,
            [FromServices] IValidator<UserInputModel> validator)
        {
            var validationResult = validator.Validate(inputModel);
            if (!validationResult.IsValid) return Results.BadRequest(validationResult.Errors);

            var sb = new StringBuilder(16);
            for (int i = 0; i < 16; i++)
            {
                sb.Append(TokenChar[Random.Shared.Next(0, TokenChar.Length)]);
            }
            var user = new User 
            {
                Name = inputModel.Name,
                Token = sb.ToString(),
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Results.Ok(new UserOutputModel(user.Id, user.Name, user.Token));
        }
    }
}
