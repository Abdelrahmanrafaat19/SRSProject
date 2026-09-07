using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using SRSProject.Infrastructure.DataContext;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Repostory
{
    public class UnitOfWork(SRSDbContext context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<
            (Type KeyType, Type EntityType),
            object> _repositories = new();

        public IGenericRepo<TKey, TEntity> Repository<TKey, TEntity>()
            where TEntity : BasicEntity<TKey>
        {
            var repositoryKey = (
                typeof(TKey),
                typeof(TEntity));

            var repository = _repositories.GetOrAdd(
                repositoryKey,
                _ => new GenericRepo<TKey, TEntity>(context));

            return (IGenericRepo<TKey, TEntity>)repository;
        }

        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
