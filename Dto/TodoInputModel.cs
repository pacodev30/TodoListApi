namespace TodoListApi.Dto
{
    public class TodoInputModel
    {
        public string Title { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}
