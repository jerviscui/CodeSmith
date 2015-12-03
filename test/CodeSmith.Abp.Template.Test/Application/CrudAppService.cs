using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

namespace CodeSmith.Abp.Template.Test.Application
{
    //TODO: Auto mappings..?
    //TODO: Select request should be sortable?

    public class CrudAppService<TRepository, TEntity, TEntityDto, TPrimaryKey, TSelectRequestInput, TCreateInput, TUpdateInput> :
        ServiceBase,
        ICrudAppService<TEntityDto, TPrimaryKey, TSelectRequestInput, TCreateInput, TUpdateInput>
        where TSelectRequestInput : DefaultPagedResultRequest
        where TRepository : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected readonly TRepository Repository;

        protected CrudAppService(TRepository repository)
        {
            Repository = repository;
        }
 
        public TEntityDto Get(IdInput<TPrimaryKey> input)
        {
            return Repository.Get(input.Id).MapTo<TEntityDto>();
        }

        public virtual PagedResultOutput<TEntityDto> GetAll(TSelectRequestInput input)
        {
            var query = CreateQueryable(input);
            PagedResultOutput<TEntityDto> output = new PagedResultOutput<TEntityDto>();
            //进行排序
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                string[] temp = input.Sorting.Trim().Split(' ');
                query = temp.Length > 0
                    ? query.OrderBy(temp[0], temp.All(t => t != "asc"))
                    : query.OrderByDescending(t => t.Id);
            }
            else
            {
                query = query.OrderByDescending(t => t.Id);
            }
            //input.MaxResultCount == 0 不进行分页
            if (input.Page <= 0)
            {
                output.TotalCount = query.Count();
                output.Items = query.MapTo<List<TEntityDto>>();
            }
            else
            {
                output.TotalCount = query.Count();
                output.Items = query.PageBy(input).MapTo<List<TEntityDto>>();
            }
            return output;
        }

        public Task<TPrimaryKey> CreateAsync(TCreateInput input)
        {
               TEntity entity = input.MapTo<TEntity>();
               return Repository.InsertAndGetIdAsync(entity);
        }

        public virtual TPrimaryKey Create(TCreateInput input)
        {
            TEntity entity = input.MapTo<TEntity>();
            //if (entity is IHasCreationTime)
            //{
            //    typeof(TEntity).GetProperty("LastModificationTime").SetValue(entity, DateTime.Now);
            //    typeof(TEntity).GetProperty("LastModifierUserId").SetValue(entity, CurrentSession.UserId);
            //}
            //if (entity is ICreationAudited)
            //{     
            //    typeof(TEntity).GetProperty("CreatorUserId").SetValue(entity, CurrentSession.UserId);
            //}
            return Repository.InsertOrUpdateAndGetId(entity);
        }

        public virtual void Update(TUpdateInput input)
        {
            var entity = Repository.Get(input.Id);
         
            if (entity is IHasCreationTime)
            {         
                DateTime oldvalue = (entity as IHasCreationTime).CreationTime;
                input.MapTo(entity);
                typeof(TEntity).GetProperty("CreationTime").SetValue(entity, oldvalue);
            }
            else
            {
                input.MapTo(entity);               
            }
                         
            Repository.Update(entity);
        }

        public virtual void Delete(IdInput<TPrimaryKey> input)
        {
            Repository.Delete(input.Id);
        }

        protected virtual IQueryable<TEntity> CreateQueryable(TSelectRequestInput input)
        {
            return Repository.GetAll();
        }
    }

    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, false);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName, bool desc)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, desc);
        }

        static class QueryableHelper<T>
        {
            private static Dictionary<string, LambdaExpression> cache = new Dictionary<string, LambdaExpression>();
            public static IQueryable<T> OrderBy(IQueryable<T> queryable, string propertyName, bool desc)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return desc ? Queryable.OrderByDescending(queryable, keySelector) : Queryable.OrderBy(queryable, keySelector);
            }
            private static LambdaExpression GetLambdaExpression(string propertyName)
            {
                if (cache.ContainsKey(propertyName)) return cache[propertyName];
                var param = Expression.Parameter(typeof(T));
                var body = Expression.Property(param, propertyName);
                var keySelector = Expression.Lambda(body, param);
                cache[propertyName] = keySelector;
                return keySelector;
            }
        }
    }
}