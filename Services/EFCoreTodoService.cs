using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Data.Models;
using TodoListApi.Dto;

namespace TodoListApi.Services
{
    public class EFCoreTodoService(TodoApiContext context) : ITodoService
    {
        private readonly TodoApiContext _context = context;

        public async Task<List<Todo>> GetAll()
        {
            var todos = await _context.Todos.ToListAsync();
            return todos;
        }
        public async Task<Todo?> GetById(int id)
        {
            var todo = await _context.Todos.Where(t =>  t.Id == id).FirstOrDefaultAsync();
            if (todo is null) return null;
            return todo;
        }
        public async Task<List<Todo>> GetActives()
        {
            var todos = await _context.Todos.Where(t => t.IsActive == true).ToListAsync();
            return todos;
        }

        public async Task<Todo> Create(TodoInputModel newTodo)
        {
            var dbTodo = new Todo 
            {
                Title = newTodo.Title,
                CreatedDate = newTodo.CreatedDate.GetValueOrDefault(),
                IsActive = newTodo.IsActive
            };
            _context.Todos.Add(dbTodo);
            await _context.SaveChangesAsync();
            return dbTodo;
        }

        public async Task<bool> Delete(int id)
        {
            return await _context.Todos.Where(t => t.Id == id).ExecuteDeleteAsync() > 0;
        }

        public async Task<bool> Update(int id, TodoInputModel newTodo)
        {
            return await _context.Todos.Where(t => t.Id == id)
                .ExecuteUpdateAsync(tod => tod
                    .SetProperty(todo => todo.Title, newTodo.Title)
                    .SetProperty(todo => todo.CreatedDate, newTodo.CreatedDate)
                    .SetProperty(todo => todo.IsActive, newTodo.IsActive)) >0;
        }
    }
}
