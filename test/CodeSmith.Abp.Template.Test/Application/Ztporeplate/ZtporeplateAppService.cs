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
    public partial class ZtporeplateAppService 
    {
        public ZtporeplateAppService(IZtporeplateRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(ZtporeplateDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(ZtporeplateDto input)
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
        protected override IQueryable<Ztporeplate> CreateQueryable(GetZtporeplatePageInput input)
        {
            IQueryable<Ztporeplate> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Temperature !=null, t => t.Temperature == input.Temperature).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OrificePlateCoefficient !=null, t => t.OrificePlateCoefficient == input.OrificePlateCoefficient).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Concentration !=null, t => t.Concentration == input.Concentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DifferentialPressure !=null, t => t.DifferentialPressure == input.DifferentialPressure).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.NegativePressure !=null, t => t.NegativePressure == input.NegativePressure).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MixedFlow !=null, t => t.MixedFlow == input.MixedFlow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PureFlow !=null, t => t.PureFlow == input.PureFlow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
