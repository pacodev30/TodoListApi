namespace TodoListApi.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public ICollection<Todo> Todos { get; set; } = [];
}
