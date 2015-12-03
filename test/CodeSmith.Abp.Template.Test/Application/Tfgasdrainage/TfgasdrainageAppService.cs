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
    public partial class TfgasdrainageAppService 
    {
        public TfgasdrainageAppService(ITfgasdrainageRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfgasdrainageDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfgasdrainageDto input)
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
        protected override IQueryable<Tfgasdrainage> CreateQueryable(GetTfgasdrainagePageInput input)
        {
            IQueryable<Tfgasdrainage> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.C !=null, t => t.C == input.C).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Kpa !=null, t => t.Kpa == input.Kpa).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Concentration !=null, t => t.Concentration == input.Concentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.COConcentration !=null, t => t.COConcentration == input.COConcentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.RunTime !=null, t => t.RunTime == input.RunTime).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MixedFlow !=null, t => t.MixedFlow == input.MixedFlow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PureFlow !=null, t => t.PureFlow == input.PureFlow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CumulativeMixedFlow !=null, t => t.CumulativeMixedFlow == input.CumulativeMixedFlow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CumulativePureFlow !=null, t => t.CumulativePureFlow == input.CumulativePureFlow).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
