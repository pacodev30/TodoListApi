namespace TodoListApi
{
    public record Todo(
        int Id, 
        string Title, 
        DateTime StartDate,
        DateTime? EndDate = null)
    {
    }
}
