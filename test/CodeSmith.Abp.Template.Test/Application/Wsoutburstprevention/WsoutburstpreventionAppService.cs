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
    public partial class WsoutburstpreventionAppService 
    {
        public WsoutburstpreventionAppService(IWsoutburstpreventionRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(WsoutburstpreventionDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(WsoutburstpreventionDto input)
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
        protected override IQueryable<Wsoutburstprevention> CreateQueryable(GetWsoutburstpreventionPageInput input)
        {
            IQueryable<Wsoutburstprevention> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressName !=null, t => t.AddressName == input.AddressName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MeasuringPoint !=null, t => t.MeasuringPoint == input.MeasuringPoint).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.K1 !=null, t => t.K1 == input.K1).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Smax !=null, t => t.Smax == input.Smax).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.H2 !=null, t => t.H2 == input.H2).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.T2 !=null, t => t.T2 == input.T2).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Type !=null, t => t.Type == input.Type).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AllowFootage !=null, t => t.AllowFootage == input.AllowFootage).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OverWhether !=null, t => t.OverWhether == input.OverWhether).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsShow !=null, t => t.IsShow == input.IsShow).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.FaceSituation !=null, t => t.FaceSituation == input.FaceSituation).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ReasonAnalysis !=null, t => t.ReasonAnalysis == input.ReasonAnalysis).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Hole !=null, t => t.Hole == input.Hole).AsQueryable();
         
            queryable = queryable.WhereIf(input.OpenHole.HasValue, t => t.OpenHole == input.OpenHole.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.FinalHole.HasValue, t => t.FinalHole == input.FinalHole.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Construction !=null, t => t.Construction == input.Construction).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Recorder !=null, t => t.Recorder == input.Recorder).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
