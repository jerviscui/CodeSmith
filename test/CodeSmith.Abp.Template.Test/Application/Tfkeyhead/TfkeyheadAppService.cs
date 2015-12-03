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
    public partial class TfkeyheadAppService 
    {
        public TfkeyheadAppService(ITfkeyheadRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfkeyheadDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfkeyheadDto input)
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
        protected override IQueryable<Tfkeyhead> CreateQueryable(GetTfkeyheadPageInput input)
        {
            IQueryable<Tfkeyhead> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Address !=null, t => t.Address == input.Address).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SupportingMethod !=null, t => t.SupportingMethod == input.SupportingMethod).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SectionDescription !=null, t => t.SectionDescription == input.SectionDescription).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.EyeNumber !=null, t => t.EyeNumber == input.EyeNumber).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DeepEye !=null, t => t.DeepEye == input.DeepEye).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MouthGas !=null, t => t.MouthGas == input.MouthGas).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.GasAfterGun !=null, t => t.GasAfterGun == input.GasAfterGun).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AllowFootage !=null, t => t.AllowFootage == input.AllowFootage).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ExcessSize !=null, t => t.ExcessSize == input.ExcessSize).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ImgUrl !=null, t => t.ImgUrl == input.ImgUrl).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
