using System.Linq.Expressions;
using TodoService.DAL.Data.Models;

namespace TodoService.DAL.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "");
        Task<IEnumerable<TEntity>> GetAllAsync();
        
        Task<TEntity?> GetByIDAsync(long id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task Delete(ulong id);
        Task Delete(TEntity entityToDelete);
    }
}
