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
    public partial class BsuseroperatelogAppService 
    {
        public BsuseroperatelogAppService(IBsuseroperatelogRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BsuseroperatelogDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsuseroperatelogDto input)
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
        protected override IQueryable<Bsuseroperatelog> CreateQueryable(GetBsuseroperatelogPageInput input)
        {
            IQueryable<Bsuseroperatelog> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MenuId !=null, t => t.MenuId == input.MenuId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ButtonId !=null, t => t.ButtonId == input.ButtonId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OperateType !=null, t => t.OperateType == input.OperateType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OperateData !=null, t => t.OperateData == input.OperateData).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CreatorUserId !=null, t => t.CreatorUserId == input.CreatorUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
