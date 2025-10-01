namespace TodoListApi.Data.Models
{
    public class TodoInputModel
    {
        public string Title { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public bool? IsActive { get; set; } = true;
        //public User User { get; set; }
        //public int UserId { get; set; }
    }
}
