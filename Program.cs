using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTodoServices();
//builder.Services.AddDbContext<TodoApiDb>(opt => opt.UseSqlServer(
//    builder.Configuration.GetConnectionString("SqlServer")));

var app = builder.Build();

app.MapGroup("/todos")
    .MapTodoEndpoints();

app.Run();
