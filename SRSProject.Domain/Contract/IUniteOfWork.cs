using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Domain.Contract
{
    public interface IUnitOfWork
    {
        IGenericRepo<TKey, TEntity> Repository<TKey, TEntity>()
            where TEntity : BasicEntity<TKey>;

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default);


        Task BeginTransactionAsync(
            CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(
            CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(
            CancellationToken cancellationToken = default);
    }
}
