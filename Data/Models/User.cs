namespace TodoListApi.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public IEnumerable<Todo>? Todos { get; set; }
}
