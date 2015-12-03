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
    public partial class ZtconstructionsiteAppService 
    {
        public ZtconstructionsiteAppService(IZtconstructionsiteRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(ZtconstructionsiteDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(ZtconstructionsiteDto input)
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
        protected override IQueryable<Ztconstructionsite> CreateQueryable(GetZtconstructionsitePageInput input)
        {
            IQueryable<Ztconstructionsite> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ConstructionSiteName !=null, t => t.ConstructionSiteName == input.ConstructionSiteName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastDrillingRig !=null, t => t.LastDrillingRig == input.LastDrillingRig).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastMonthlyPlan !=null, t => t.LastMonthlyPlan == input.LastMonthlyPlan).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastTeam !=null, t => t.LastTeam == input.LastTeam).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PoreSize !=null, t => t.PoreSize == input.PoreSize).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.State !=null, t => t.State == input.State).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDeleted !=null, t => t.IsDeleted == input.IsDeleted).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.StartMonthlyPlan !=null, t => t.StartMonthlyPlan == input.StartMonthlyPlan).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
