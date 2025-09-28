using TodoListApi.Data.Models;

namespace TodoListApi.Services
{
    public interface ITodoService
    {
        Task<List<TodoOutputModel>> GetAll();
        Task<TodoOutputModel?> GetById(int id);
        Task<List<TodoOutputModel>> GetActives();
        Task<TodoOutputModel> Create(TodoInputModel newTodo);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, TodoInputModel todoUpdated);
    }
}
