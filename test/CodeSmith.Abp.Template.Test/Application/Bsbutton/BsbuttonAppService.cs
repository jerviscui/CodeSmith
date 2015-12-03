using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CodeSmith.Abp.Template.Test.Core;

// ReSharper disable once CheckNamespace
namespace CodeSmith.Abp.Template.Test.Application
{
    public partial class BsbuttonAppService 
    {
        public BsbuttonAppService(IBsbuttonRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BsbuttonDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsbuttonDto input)
        {
             base.Update(input);
        }
        
        public override void Delete(IdInput<int> input)
        {
            base.Delete(input);
        }
        
        #endregion
        
        #region protected
        
        /// <summary>
        /// 数据检索
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<Bsbutton> CreateQueryable(GetBsbuttonPageInput input)
        {
            IQueryable<Bsbutton> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Text !=null, t => t.Text == input.Text).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.RunEvent !=null, t => t.RunEvent == input.RunEvent).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IconClass !=null, t => t.IconClass == input.IconClass).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CreatorUserId !=null, t => t.CreatorUserId == input.CreatorUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.LastModificationTime.HasValue, t => t.LastModificationTime == input.LastModificationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastModifierUserId !=null, t => t.LastModifierUserId == input.LastModifierUserId).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
