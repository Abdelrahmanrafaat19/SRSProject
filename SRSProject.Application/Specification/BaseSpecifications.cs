using SRSProject.Domain.Contract;
using SRSProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SRSProject.Application.Specification
{
    public class BaseSpecifications<Tkey, TEntity> : ISpecificationRepo<Tkey, TEntity> where TEntity : BasicEntity<Tkey>
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; private set; } = [];



        public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }


        #region Criteria
        public Expression<Func<TEntity, bool>> Criteria { get; private set; }
        protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteria = null)
        {
            Criteria = criteria;
        }
        #endregion


        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDecending { get; private set; }



        public void AddOrderByAsyndec(Expression<Func<TEntity, object>> orderByAsyndec)
        {
            OrderBy = orderByAsyndec;
        }

        public void AddOrderByDecendc(Expression<Func<TEntity, object>> orderByDecendec)
        {
            OrderByDecending = orderByDecendec;
        }

        public int Skip { get; private set; }

        public int Take { get; private set; }
        public void EnablePagination(int pageSize = 10, int pageIndex = 1, bool IsPagination = false)
        {
            IsPagingEnabled = IsPagination;
            if (IsPagingEnabled)
            {
                if (pageSize > 20 || pageSize < 1)
                {
                    Take = 20;
                }
                else
                {
                    Take = pageSize;
                }
                if (pageIndex <= 0)
                {
                    pageIndex = 1;
                }


                Skip = (pageIndex - 1) * pageSize;
            }
        }
        public bool IsPagingEnabled { get; private set; }
    }
}
