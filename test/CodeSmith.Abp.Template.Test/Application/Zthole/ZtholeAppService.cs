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
    public partial class ZtholeAppService 
    {
        public ZtholeAppService(IZtholeRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(ZtholeDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(ZtholeDto input)
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
        protected override IQueryable<Zthole> CreateQueryable(GetZtholePageInput input)
        {
            IQueryable<Zthole> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ConstructionSiteId !=null, t => t.ConstructionSiteId == input.ConstructionSiteId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DrillingRig !=null, t => t.DrillingRig == input.DrillingRig).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DayStage !=null, t => t.DayStage == input.DayStage).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DrillingFootage !=null, t => t.DrillingFootage == input.DrillingFootage).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DayTotal !=null, t => t.DayTotal == input.DayTotal).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MonthTotal !=null, t => t.MonthTotal == input.MonthTotal).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CurrentMonthPlan !=null, t => t.CurrentMonthPlan == input.CurrentMonthPlan).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PersonInCharge !=null, t => t.PersonInCharge == input.PersonInCharge).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CurrentPeopleNum !=null, t => t.CurrentPeopleNum == input.CurrentPeopleNum).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.No !=null, t => t.No == input.No).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.HoleType !=null, t => t.HoleType == input.HoleType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PoreSize !=null, t => t.PoreSize == input.PoreSize).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Position !=null, t => t.Position == input.Position).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DipAngle !=null, t => t.DipAngle == input.DipAngle).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DesignHoleDepth !=null, t => t.DesignHoleDepth == input.DesignHoleDepth).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Coal !=null, t => t.Coal == input.Coal).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Rock !=null, t => t.Rock == input.Rock).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ConstructionSituation !=null, t => t.ConstructionSituation == input.ConstructionSituation).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SpaceBefore !=null, t => t.SpaceBefore == input.SpaceBefore).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsEndHole !=null, t => t.IsEndHole == input.IsEndHole).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Team !=null, t => t.Team == input.Team).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
