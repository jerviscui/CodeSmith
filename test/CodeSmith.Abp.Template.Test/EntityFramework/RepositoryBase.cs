using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CodeSmith.Abp.Template.Test.EntityFramework
{
    /// <summary>
    /// We declare a base repository class for our application.
    /// It inherits from <see cref="EfRepositoryBase{TDbContext,TEntity,TPrimaryKey}"/>.
    /// We can add here common methods for all our repositories.
    /// </summary>
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<YtQdYtsfContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected RepositoryBase(IDbContextProvider<YtQdYtsfContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    /// <summary>
    /// A shortcut of <see cref="RepositoryBase{TEntity,TPrimaryKey}"/> for Entities with primary key type <see cref="int"/>.
    /// </summary>
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected RepositoryBase(IDbContextProvider<YtQdYtsfContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
