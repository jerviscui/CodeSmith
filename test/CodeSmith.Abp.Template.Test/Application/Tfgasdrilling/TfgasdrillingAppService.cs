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
    public partial class TfgasdrillingAppService 
    {
        public TfgasdrillingAppService(ITfgasdrillingRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfgasdrillingDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfgasdrillingDto input)
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
        protected override IQueryable<Tfgasdrilling> CreateQueryable(GetTfgasdrillingPageInput input)
        {
            IQueryable<Tfgasdrilling> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreateDateTime.HasValue, t => t.CreateDateTime == input.CreateDateTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DrillingId !=null, t => t.DrillingId == input.DrillingId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.TValue !=null, t => t.TValue == input.TValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.COValue !=null, t => t.COValue == input.COValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDeleted !=null, t => t.IsDeleted == input.IsDeleted).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
