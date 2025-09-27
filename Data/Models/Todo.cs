namespace TodoListApi.Data.Models;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; }
    public bool IsActive { get; set; }
}
