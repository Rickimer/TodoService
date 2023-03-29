using AutoMapper;
using TodoService.BLL.DTO;
using TodoService.DAL.Data.Models;

namespace TodoService.BLL.Shared
{
    public class BllMappingProfile : Profile
    {
        public BllMappingProfile()
        {
            CreateMap<Todo, TodoDto>()
                .ReverseMap();
        }
    }
}
