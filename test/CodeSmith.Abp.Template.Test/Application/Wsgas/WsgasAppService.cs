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
    public partial class WsgasAppService 
    {
        public WsgasAppService(IWsgasRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(WsgasDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(WsgasDto input)
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
        protected override IQueryable<Wsgas> CreateQueryable(GetWsgasPageInput input)
        {
            IQueryable<Wsgas> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AirVolume !=null, t => t.AirVolume == input.AirVolume).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.WorkingFaceCH4 !=null, t => t.WorkingFaceCH4 == input.WorkingFaceCH4).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.UpperCornerCH4 !=null, t => t.UpperCornerCH4 == input.UpperCornerCH4).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.GoafCH4 !=null, t => t.GoafCH4 == input.GoafCH4).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.BackRomanticAvg !=null, t => t.BackRomanticAvg == input.BackRomanticAvg).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.BackRomanticMax !=null, t => t.BackRomanticMax == input.BackRomanticMax).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MaxGushGas !=null, t => t.MaxGushGas == input.MaxGushGas).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AverageExhaustAir !=null, t => t.AverageExhaustAir == input.AverageExhaustAir).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AverageGushGas !=null, t => t.AverageGushGas == input.AverageGushGas).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PureFlow !=null, t => t.PureFlow == input.PureFlow).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
