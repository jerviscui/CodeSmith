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
    public partial class TfkindlefirewallAppService 
    {
        public TfkindlefirewallAppService(ITfkindlefirewallRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfkindlefirewallDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfkindlefirewallDto input)
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
        protected override IQueryable<Tfkindlefirewall> CreateQueryable(GetTfkindlefirewallPageInput input)
        {
            IQueryable<Tfkindlefirewall> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.EnclosedWallLedgerId !=null, t => t.EnclosedWallLedgerId == input.EnclosedWallLedgerId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.GasConcentration !=null, t => t.GasConcentration == input.GasConcentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.COConcentration !=null, t => t.COConcentration == input.COConcentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CO2Concentration !=null, t => t.CO2Concentration == input.CO2Concentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Temperature !=null, t => t.Temperature == input.Temperature).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.O2Concentration !=null, t => t.O2Concentration == input.O2Concentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OutsideGasConcentration !=null, t => t.OutsideGasConcentration == input.OutsideGasConcentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OutsideCO2Concentration !=null, t => t.OutsideCO2Concentration == input.OutsideCO2Concentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OutsideCOConcentration !=null, t => t.OutsideCOConcentration == input.OutsideCOConcentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.OutsideTConcentration !=null, t => t.OutsideTConcentration == input.OutsideTConcentration).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DifferentialPressure !=null, t => t.DifferentialPressure == input.DifferentialPressure).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.WaterTemperature !=null, t => t.WaterTemperature == input.WaterTemperature).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsSensor !=null, t => t.IsSensor == input.IsSensor).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsBeamTube !=null, t => t.IsBeamTube == input.IsBeamTube).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsCOSensor !=null, t => t.IsCOSensor == input.IsCOSensor).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDrainage !=null, t => t.IsDrainage == input.IsDrainage).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
