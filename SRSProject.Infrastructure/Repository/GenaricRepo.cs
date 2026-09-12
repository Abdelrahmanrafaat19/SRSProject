using Microsoft.EntityFrameworkCore;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using SRSProject.Infrastructure.DataContext;
using SRSProject.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SRSProject.Infrastructure.Repostory
{
    public class GenericRepo<TKey, TEntity>(SRSDbContext context)
        : IGenericRepo<TKey, TEntity>
        where TEntity : BasicEntity<TKey>
    {
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(
            TKey id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(
                [id],
                cancellationToken);
        }

        public async Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task AddAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(
                entities,
                cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
   

        public async Task<TEntity?> FirstOrDefaultAsyncWithSpecification(
            ISpecificationRepo<TKey,TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

       

        public async Task<int> CountAsyncWithSpecification(
            ISpecificationRepo<TKey,TEntity> specification,
            CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = _dbSet;

            if (specification.Criteria is not null)
            {
                query = query.Where(specification.Criteria);
            }

            return await query.CountAsync(cancellationToken);
        }

        private IQueryable<TEntity> ApplySpecification(
            ISpecificationRepo<TKey,TEntity> specification)
        {
           return  SpecificationEvaluator
         .GetQueryable<TEntity, TKey>(
             _dbSet.AsQueryable(),
             specification);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsyncWithSpecification(ISpecificationRepo<TKey, TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AnyAsyncWithSpecification(ISpecificationRepo<TKey, TEntity> specification, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(specification)
              .AsNoTracking()
              .AnyAsync(cancellationToken);
        }
    }
}
