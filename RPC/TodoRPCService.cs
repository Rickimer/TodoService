using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TodoService;
using TodoService.BLL.DTO;
using TodoService.BLL.Services;
using TodoService.BLL.Shared;

namespace RPC
{
    public class TodoRPCService : TodoRPC.TodoRPCBase
    {
        private readonly ILogger<TodoRPCService> _logger;
        private readonly IMapper _mapper;
        IBllTodoService _todoService;
        private readonly Tenants _tenants;

        public TodoRPCService(ILogger<TodoRPCService> logger, IMapper mapper, IBllTodoService todoService           
            ,IOptions<Tenants> tenants
            )
        {
            _logger = logger;
            _mapper = mapper;
            _todoService = todoService;
            _tenants = tenants.Value;
        }

        public override async Task<RPCTodoId?> CreateTodo(RPCCreateTodo todo, ServerCallContext context)
        {
            CheckMetadata(context.RequestHeaders);
            try
            {
                var dto = _mapper.Map<TodoDto>(todo);
                var id = await _todoService.AddTodo(dto);
                return new RPCTodoId { Id = id};
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                _logger.LogError(error);                
                throw new RpcException(new Status(StatusCode.Internal, error));
            }            
        }

        public override async Task<Empty> UpdateTodo(RPCUpdateTodo todo, ServerCallContext context)
        {
            CheckMetadata(context.RequestHeaders);
            try
            {
                var dto = _mapper.Map<TodoDto>(todo);
                var id = await _todoService.AddTodo(dto);
                return new Empty();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                _logger.LogError(error);
                throw new RpcException(new Status(StatusCode.Internal, error));
            }
        }

        public override async Task<Empty> DeleteTodo(RPCDeleteTodo todo, ServerCallContext context)
        {
            CheckMetadata(context.RequestHeaders);
            try
            {                
                await _todoService.DeleteTodo(todo.TodoId);
                return new Empty();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                _logger.LogError(error);
                throw new RpcException(new Status(StatusCode.Internal, error));
            }
        }

        public override async Task<RPCTodos> GetTodos(RPCUser request, ServerCallContext context)
        {
            CheckMetadata(context.RequestHeaders);

            var todos = (await _todoService.GetAllAsync(new BllInputTodoDto { UserId = request.UserId })).ToList();            
            
            var rpcDto = new RPCTodos();
            foreach (var d in todos)
            {
                var todo = new RPCTodo { Id = d.Id, Title = d.Title,  };
                rpcDto.Todos.Add(todo);
            }            

            //return Task.FromResult(rpcDto);
            return rpcDto;
        }

        public override Task<Empty> HealthCheck(Empty input, ServerCallContext context)
        {            
            return Task.FromResult(new Empty());            
        }

        public override Task<SystemReportResult> SystemReport(Empty input, ServerCallContext context)
        {
            return Task.FromResult(new SystemReportResult());
        }

        private void CheckMetadata(Metadata data)
        {
            var metadataEntry = data.FirstOrDefault(m => string.Equals(m.Key, "todoclient", StringComparison.Ordinal));
            if (metadataEntry == null || metadataEntry.Equals(default(Metadata.Entry)) || metadataEntry.Value == null)
            {                
                throw new RpcException(new Status(StatusCode.Internal, "No Client athentication data"));
            }
            
            var client = metadataEntry.Value.Split(",");
            if (client.Length != 2)
            {                 
                throw new RpcException(new Status(StatusCode.Internal, "Error structure metadate"));
            }

            switch (client[0])
            {
                case "AuthServer":
                    if (!_tenants.AuthServerToken.Equals(client[1]))
                        throw new RpcException(new Status(StatusCode.Internal, "Incorrect password AuthServer tenant"));
                    break;
                default:
                    throw new RpcException(new Status(StatusCode.Internal, "No eligible client found"));
            }
        }
    }
}
