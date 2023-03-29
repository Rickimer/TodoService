namespace TodoService.BLL.DTO
{
    public class TodoDto
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public string Title { get; set; } = String.Empty;
        public bool IsCompleted { get; set; }
        public DateTime Created { get; set; }
    }
}
