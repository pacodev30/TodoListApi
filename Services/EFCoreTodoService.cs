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

        public async Task<List<TodoOutputModel>> GetAll()
        {
            var todos = await _context.Todos.ToListAsync();
            var outputTodos = todos.ConvertAll(ToOutputModel);
            return outputTodos;
            
        }
        public async Task<TodoOutputModel?> GetById(int id)
        {
            var todo = await _context.Todos.Where(t =>  t.Id == id).FirstOrDefaultAsync();
            if (todo is null) return null;
            return ToOutputModel(todo);
        }
        public async Task<List<TodoOutputModel>> GetActives()
        {
            var todos = await _context.Todos.Where(t => t.IsActive == true).ToListAsync();
            var outputTodos = todos.ConvertAll(ToOutputModel);
            return outputTodos;
        }

        public async Task<TodoOutputModel> Create(TodoInputModel newTodo)
        {
            var dbTodo = new Todo
            {
                Title = newTodo.Title,
                CreatedDate = newTodo.CreatedDate.GetValueOrDefault(),
                IsActive = newTodo.IsActive.GetValueOrDefault()
            };
            _context.Todos.Add(dbTodo);
            await _context.SaveChangesAsync();
            return ToOutputModel(dbTodo);
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
