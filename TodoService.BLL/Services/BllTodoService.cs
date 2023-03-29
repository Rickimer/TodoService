using TodoService.BLL.DTO;
using TodoService.DAL.Data.Repository;
using TodoService.DAL.Data.Models;
using AutoMapper;

namespace TodoService.BLL.Services
{
    public class BllTodoService : IBllTodoService
    {
        private readonly IMapper _mapper;
        IRepository<Todo> _todoRepository;

        public BllTodoService(IMapper mapper, IRepository<Todo> todoRepository)
        {
            _mapper = mapper;
            _todoRepository = todoRepository;
        }

        public async Task<ulong> AddTodo(TodoDto dto)
        {
            var entity = _mapper.Map<Todo>(dto);
            entity.Created = DateTime.Now;
            await _todoRepository.AddAsync(entity);
            return entity.Id;
        }

        public async Task DeleteTodo(ulong id)
        {
            await _todoRepository.Delete(id);
        }

        public async Task UpdateTodo(TodoDto dto)
        {
            var todo = _mapper.Map<Todo>(dto);
            await _todoRepository.UpdateAsync(todo);
        }


        public async Task<IEnumerable<TodoDto>> GetAllAsync(BllInputTodoDto dto)
        {            
            var list = (await _todoRepository.GetAsync())
                .Where(w => w.UserId == dto.UserId).ToList();
            
            var result = list.Select(e => _mapper.Map<TodoDto>(e));

            return result;
        }
    }
}
