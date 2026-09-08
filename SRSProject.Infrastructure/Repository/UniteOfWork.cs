using Microsoft.EntityFrameworkCore.Storage;
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
        private IDbContextTransaction? _transaction;

        public async Task BeginTransactionAsync(
             CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
            {
                throw new InvalidOperationException(
                    "A transaction is already active.");
            }

            _transaction =
                await context.Database.BeginTransactionAsync(
                    cancellationToken);
        }

        public async Task CommitTransactionAsync(
          CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException(
                    "There is no active transaction.");
            }

            try
            {
                await _transaction.CommitAsync(
                    cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }


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

        public async Task RollbackTransactionAsync(
        CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
            {
                return;
            }

            try
            {
                await _transaction.RollbackAsync(
                    cancellationToken);
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        private async Task DisposeTransactionAsync()
        {
            if (_transaction is null)
            {
                return;
            }

            await _transaction.DisposeAsync();

            _transaction = null;
        }
    }
}
