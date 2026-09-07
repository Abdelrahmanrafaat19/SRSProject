using Microsoft.EntityFrameworkCore;
using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRSProject.Infrastructure.Repository
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQueryable<TEntity, Tkey>
         (
            IQueryable<TEntity> inputQuery,
            ISpecificationRepo<Tkey, TEntity> specification) where TEntity : BasicEntity<Tkey>
        {
            IQueryable<TEntity> query = inputQuery;

            // Apply filtering
            if (specification.Criteria is not null)
            {
                query = query.Where(
                    specification.Criteria);
            }

            // Apply navigation-property includes
            foreach (var includeExpression
                     in specification.Includes)
            {
                query = query.Include(
                    includeExpression);
            }

      
            if (specification.OrderBy is not null)
            {
                query = query.OrderBy(
                    specification.OrderBy);
            }
            else if (
                specification.OrderByDecending is not null)
            {
                query = query.OrderByDescending(
                    specification.OrderByDecending);
            }

            // Apply pagination
            if (specification.IsPagingEnabled)
            {
                query = query
                    .Skip(specification.Skip)
                    .Take(specification.Take);
            }

            return query;
        }

    }
}
