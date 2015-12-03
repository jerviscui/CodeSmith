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
    public partial class BsmenuAppService 
    {
        public BsmenuAppService(IBsmenuRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BsmenuDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsmenuDto input)
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
        protected override IQueryable<Bsmenu> CreateQueryable(GetBsmenuPageInput input)
        {
            IQueryable<Bsmenu> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MenuText !=null, t => t.MenuText == input.MenuText).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MenuUrl !=null, t => t.MenuUrl == input.MenuUrl).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Sort !=null, t => t.Sort == input.Sort).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ParentMenuId !=null, t => t.ParentMenuId == input.ParentMenuId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ParentMenuIds !=null, t => t.ParentMenuIds == input.ParentMenuIds).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsMain !=null, t => t.IsMain == input.IsMain).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsInline !=null, t => t.IsInline == input.IsInline).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsActive !=null, t => t.IsActive == input.IsActive).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OpenEvent !=null, t => t.OpenEvent == input.OpenEvent).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CloseEvent !=null, t => t.CloseEvent == input.CloseEvent).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remark !=null, t => t.Remark == input.Remark).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
