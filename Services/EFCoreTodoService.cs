using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Data.Models;

namespace TodoListApi.Services
{
    public class EFCoreTodoService(TodoApiContext context) : ITodoService
    {
        private readonly TodoApiContext _context = context;
        private TodoOutputModel ToOutputModel(Todo todo)
           =>  new(todo.Id, todo.Title, todo.CreatedDate, todo.IsActive);

        public async Task<List<TodoOutputModel>> GetAll(int userId)
        {
            var todos = await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
            var outputTodos = todos.ConvertAll(ToOutputModel);
            return outputTodos;
            
        }
        public async Task<TodoOutputModel?> GetById(int id, int userId)
        {
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (todo is null) return null;
            return ToOutputModel(todo);
        }
        public async Task<List<TodoOutputModel>> GetActives(int userId)
        {
            var todos = await _context.Todos.Where(t => t.IsActive == true && t.UserId == userId).ToListAsync();
            var outputTodos = todos.ConvertAll(ToOutputModel);
            return outputTodos;
        }

        public async Task<TodoOutputModel> Add(TodoInputModel newTodo, int userID)
        {
            var dbTodo = new Todo
            {
                UserId = userID,
                Title = newTodo.Title,
                CreatedDate = newTodo.CreatedDate.GetValueOrDefault(),
                IsActive = newTodo.IsActive.GetValueOrDefault()
            };
            _context.Todos.Add(dbTodo);
            await _context.SaveChangesAsync();
            return ToOutputModel(dbTodo);
        }

        public async Task<bool> Delete(int id, int userId)
        {
            return await _context.Todos.Where(t => t.Id == id && t.UserId == userId).ExecuteDeleteAsync() > 0;
        }

        public async Task<bool> Update(int id, int userId, TodoInputModel newTodo)
        {
            var dbTodo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (dbTodo is null) return false;

            dbTodo.Title = newTodo.Title;
            dbTodo.CreatedDate = newTodo.CreatedDate.GetValueOrDefault();
            dbTodo.IsActive = newTodo.IsActive.GetValueOrDefault();

            _context.Todos.Update(dbTodo);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
