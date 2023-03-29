using AutoMapper;
using TodoService;
using TodoService.BLL.DTO;

namespace RPC.Shared
{
    public class RPCMappingProfile : Profile
    {
        public RPCMappingProfile()
        {
            CreateMap<RPCCreateTodo, TodoDto>()
                .ReverseMap();
        }
    }
}
