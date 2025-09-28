using TodoListApi.Data.Models;
using TodoListApi.Dto;

namespace TodoListApi.Services
{
    public interface ITodoService
    {
        Task<List<Todo>> GetAll();
        Task<Todo?> GetById(int id);
        Task<List<Todo>> GetActives();
        Task<Todo> Create(TodoInputModel newTodo);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, TodoInputModel todoUpdated);
    }
}
