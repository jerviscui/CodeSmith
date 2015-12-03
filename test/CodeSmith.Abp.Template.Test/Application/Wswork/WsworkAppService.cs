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
    public partial class WsworkAppService 
    {
        public WsworkAppService(IWsworkRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(WsworkDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(WsworkDto input)
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
        protected override IQueryable<Wswork> CreateQueryable(GetWsworkPageInput input)
        {
            IQueryable<Wswork> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SameDay !=null, t => t.SameDay == input.SameDay).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Accumulative !=null, t => t.Accumulative == input.Accumulative).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MultipleWork !=null, t => t.MultipleWork == input.MultipleWork).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Project !=null, t => t.Project == input.Project).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SystemStatus !=null, t => t.SystemStatus == input.SystemStatus).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Focus !=null, t => t.Focus == input.Focus).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
