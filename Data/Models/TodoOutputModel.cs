namespace TodoListApi.Data.Models
{
    public record TodoOutputModel(int Id, string Title, DateTime? CreatedDate, bool? IsActive)
    {
    }
}
