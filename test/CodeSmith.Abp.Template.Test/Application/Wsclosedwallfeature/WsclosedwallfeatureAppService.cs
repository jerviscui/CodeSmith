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
    public partial class WsclosedwallfeatureAppService 
    {
        public WsclosedwallfeatureAppService(IWsclosedwallfeatureRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(WsclosedwallfeatureDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(WsclosedwallfeatureDto input)
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
        protected override IQueryable<Wsclosedwallfeature> CreateQueryable(GetWsclosedwallfeaturePageInput input)
        {
            IQueryable<Wsclosedwallfeature> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LocationType !=null, t => t.LocationType == input.LocationType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Co !=null, t => t.Co == input.Co).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Co2 !=null, t => t.Co2 == input.Co2).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.C2h4 !=null, t => t.C2h4 == input.C2h4).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Ch4 !=null, t => t.Ch4 == input.Ch4).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.C2h6 !=null, t => t.C2h6 == input.C2h6).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.C2h2 !=null, t => t.C2h2 == input.C2h2).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.O2 !=null, t => t.O2 == input.O2).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.N2 !=null, t => t.N2 == input.N2).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.T !=null, t => t.T == input.T).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Statistic !=null, t => t.Statistic == input.Statistic).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Nitrogen !=null, t => t.Nitrogen == input.Nitrogen).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Inhibitor !=null, t => t.Inhibitor == input.Inhibitor).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Grouting !=null, t => t.Grouting == input.Grouting).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Other !=null, t => t.Other == input.Other).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.RiskSituation !=null, t => t.RiskSituation == input.RiskSituation).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
