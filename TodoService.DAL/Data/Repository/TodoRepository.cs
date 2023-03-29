using TodoService.DAL.Data.Models;

namespace TodoService.DAL.Data.Repository
{
    public class TodoRepository : GeneralRepository<Todo, TodoContext>
    {
        public TodoRepository(TodoContext context) : base(context)
        {
        }
    }
}
