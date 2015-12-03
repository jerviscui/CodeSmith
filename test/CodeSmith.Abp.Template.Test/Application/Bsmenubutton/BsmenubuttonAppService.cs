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
    public partial class BsmenubuttonAppService 
    {
        public BsmenubuttonAppService(IBsmenubuttonRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BsmenubuttonDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsmenubuttonDto input)
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
        protected override IQueryable<Bsmenubutton> CreateQueryable(GetBsmenubuttonPageInput input)
        {
            IQueryable<Bsmenubutton> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ButtonId !=null, t => t.ButtonId == input.ButtonId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MenuId !=null, t => t.MenuId == input.MenuId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ButtonText !=null, t => t.ButtonText == input.ButtonText).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.RunEvent !=null, t => t.RunEvent == input.RunEvent).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IconClass !=null, t => t.IconClass == input.IconClass).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsActive !=null, t => t.IsActive == input.IsActive).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Sort !=null, t => t.Sort == input.Sort).AsQueryable();
         
            queryable = queryable.WhereIf(input.LastModificationTime.HasValue, t => t.LastModificationTime == input.LastModificationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastModifierUserId !=null, t => t.LastModifierUserId == input.LastModifierUserId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CreatorUserId !=null, t => t.CreatorUserId == input.CreatorUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
