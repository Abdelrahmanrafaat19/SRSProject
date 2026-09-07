using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SRSProject.Domain.Contract
{
    public interface ISpecificationRepo<TKey, TEntity> where TEntity : BasicEntity<TKey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; }
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDecending { get; }
        public int Skip { get; }
        public int Take { get; }
        public bool IsPagingEnabled { get; }
    }
}
