using TodoListApi.Data.Models;

namespace TodoListApi.Services
{
    public interface ITodoService
    {
        Task<List<TodoOutputModel>> GetAll(int userID);
        Task<TodoOutputModel?> GetById(int id, int userID);
        Task<List<TodoOutputModel>> GetActives(int userID);
        Task<TodoOutputModel> Add(TodoInputModel newTodo, int userID);
        Task<bool> Delete(int id, int userID);
        Task<bool> Update(int id, int userID, TodoInputModel todoUpdated);
    }
}
