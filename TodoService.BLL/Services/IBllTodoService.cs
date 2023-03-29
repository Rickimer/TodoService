using TodoService.BLL.DTO;

namespace TodoService.BLL.Services
{
    public interface IBllTodoService
    {
        Task<ulong> AddTodo(TodoDto dto);
        Task UpdateTodo(TodoDto dto);
        Task DeleteTodo(ulong id);
        Task<IEnumerable<TodoDto>> GetAllAsync(BllInputTodoDto dto);
    }
}
