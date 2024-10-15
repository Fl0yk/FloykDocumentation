﻿using System.Linq.Expressions;

namespace Forum.Infrastructure.Specifications;

public abstract class Specification<TEntity>
    where TEntity : class
{
    protected Specification(Expression<Func<TEntity, bool>>? criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>>? Criteria { get; }

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];

    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    protected void AddInclude(Expression<Func<TEntity, object>> include)
    {
        IncludeExpressions.Add(include);
    }

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
    }
}