using AutoMapper;
using Moq;
using RPC.Shared;
using System.Linq.Expressions;
using TodoService.BLL.DTO;
using TodoService.BLL.Services;
using TodoService.BLL.Shared;
using TodoService.DAL.Data.Models;
using TodoService.DAL.Data.Repository;

namespace TestProject
{
    public class BLLTests
    {
        [Fact]
        public async Task GetAllRepoTest()
        {
            // Arrange
            var mock = new Mock<IRepository<Todo>>();
            var userId = 1;
            mock.Setup(repo => repo.GetAsync(null, null, "")).Returns(Task.FromResult(GetTodos()));            

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BllMappingProfile());                
            });
            var mapper = mockMapper.CreateMapper();

            //Act
            var service = new BllTodoService(mapper, mock.Object);
            var list = await service.GetAllAsync(new BllInputTodoDto { UserId = 1});
            
            //Assert
            Assert.Equal(GetTodos().Where(e=>e.UserId == userId).ToList().Count, list.Count());
        }

        private IEnumerable<Todo> GetTodos()
        {
            var todos = new List<Todo>
            {
                new Todo { Title="doit1", UserId = 1},
                new Todo { Title="doit2", UserId = 1},
                new Todo { Title="doit3", UserId = 2},
                new Todo { Title="doit4", UserId = 1},
            };
            return todos;
        }
    }
}