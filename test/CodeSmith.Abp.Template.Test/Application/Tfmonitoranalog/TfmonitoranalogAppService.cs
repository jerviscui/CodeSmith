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
    public partial class TfmonitoranalogAppService 
    {
        public TfmonitoranalogAppService(ITfmonitoranalogRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfmonitoranalogDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfmonitoranalogDto input)
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
        protected override IQueryable<Tfmonitoranalog> CreateQueryable(GetTfmonitoranalogPageInput input)
        {
            IQueryable<Tfmonitoranalog> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SysType !=null, t => t.SysType == input.SysType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MonitorType !=null, t => t.MonitorType == input.MonitorType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MaxValue !=null, t => t.MaxValue == input.MaxValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MaxMomentValue !=null, t => t.MaxMomentValue == input.MaxMomentValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MinValue !=null, t => t.MinValue == input.MinValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MinMomentValue !=null, t => t.MinMomentValue == input.MinMomentValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DailyAverageValue !=null, t => t.DailyAverageValue == input.DailyAverageValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OutCount !=null, t => t.OutCount == input.OutCount).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
