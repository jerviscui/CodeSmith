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
    public partial class CmacquisitionlogAppService 
    {
        public CmacquisitionlogAppService(ICmacquisitionlogRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(CmacquisitionlogDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(CmacquisitionlogDto input)
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
        protected override IQueryable<Cmacquisitionlog> CreateQueryable(GetCmacquisitionlogPageInput input)
        {
            IQueryable<Cmacquisitionlog> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ObjectName !=null, t => t.ObjectName == input.ObjectName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ObjectCode !=null, t => t.ObjectCode == input.ObjectCode).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Contents !=null, t => t.Contents == input.Contents).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
