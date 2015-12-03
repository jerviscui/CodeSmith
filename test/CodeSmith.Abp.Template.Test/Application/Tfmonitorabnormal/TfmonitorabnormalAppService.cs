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
    public partial class TfmonitorabnormalAppService 
    {
        public TfmonitorabnormalAppService(ITfmonitorabnormalRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfmonitorabnormalDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfmonitorabnormalDto input)
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
        protected override IQueryable<Tfmonitorabnormal> CreateQueryable(GetTfmonitorabnormalPageInput input)
        {
            IQueryable<Tfmonitorabnormal> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Exception !=null, t => t.Exception == input.Exception).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AbnormalContext !=null, t => t.AbnormalContext == input.AbnormalContext).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressName !=null, t => t.AddressName == input.AddressName).AsQueryable();
         
            queryable = queryable.WhereIf(input.StartTime.HasValue, t => t.StartTime == input.StartTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.EndTime.HasValue, t => t.EndTime == input.EndTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MaxAlarmValue !=null, t => t.MaxAlarmValue == input.MaxAlarmValue).AsQueryable();
         
            queryable = queryable.WhereIf(input.MaxAlarmTime.HasValue, t => t.MaxAlarmTime == input.MaxAlarmTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Reason !=null, t => t.Reason == input.Reason).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
