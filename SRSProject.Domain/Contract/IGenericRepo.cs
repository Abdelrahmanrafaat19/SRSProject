using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SRSProject.Domain.Contract
{
    public interface IGenericRepo<TKey, TEntity>
       where TEntity : BasicEntity<TKey>
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TEntity>> GetAllAsyncWithSpecification(
            ISpecificationRepo<TKey, TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<TEntity?> GetByIdAsync(
            TKey id,
            CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsyncWithSpecification(
            ISpecificationRepo<TKey,TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<bool> AnyAsyncWithSpecification(
            ISpecificationRepo<TKey, TEntity> specification,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        Task<int> CountAsyncWithSpecification(
            ISpecificationRepo<TKey, TEntity>
            specification,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default);

        Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        Task Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
