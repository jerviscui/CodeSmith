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
    public partial class ZtfieldiinspectionAppService 
    {
        public ZtfieldiinspectionAppService(IZtfieldiinspectionRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(ZtfieldiinspectionDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(ZtfieldiinspectionDto input)
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
        protected override IQueryable<Ztfieldiinspection> CreateQueryable(GetZtfieldiinspectionPageInput input)
        {
            IQueryable<Ztfieldiinspection> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Address !=null, t => t.Address == input.Address).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DrillingField !=null, t => t.DrillingField == input.DrillingField).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Distance !=null, t => t.Distance == input.Distance).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Drainage !=null, t => t.Drainage == input.Drainage).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Method !=null, t => t.Method == input.Method).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Concentration !=null, t => t.Concentration == input.Concentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MixedFlow !=null, t => t.MixedFlow == input.MixedFlow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PureFlow !=null, t => t.PureFlow == input.PureFlow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Temperature !=null, t => t.Temperature == input.Temperature).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
