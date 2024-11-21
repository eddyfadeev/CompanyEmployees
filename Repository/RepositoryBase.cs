using System.Linq.Expressions;
using Contracts.Repository;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity>  where TEntity : class
{
    protected RepositoryContext RepositoryContext;

    public RepositoryBase(RepositoryContext context) =>
        RepositoryContext = context;

    public IQueryable<TEntity> FindAll(bool trackChanges) =>
        !trackChanges
            ? RepositoryContext
                .Set<TEntity>()
                .AsNoTracking()
            : RepositoryContext
                .Set<TEntity>();

    public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression, bool trackChanges) =>
        !trackChanges
            ? RepositoryContext
                .Set<TEntity>()
                .Where(expression).AsNoTracking()
            : RepositoryContext
                .Set<TEntity>()
                .Where(expression);

    public void Create(TEntity entity) =>
        RepositoryContext
            .Set<TEntity>()
            .Add(entity);

    public void Update(TEntity entity) =>
        RepositoryContext
            .Set<TEntity>()
            .Update(entity);

    public void Delete(TEntity entity) =>
        RepositoryContext
            .Set<TEntity>()
            .Remove(entity);
}