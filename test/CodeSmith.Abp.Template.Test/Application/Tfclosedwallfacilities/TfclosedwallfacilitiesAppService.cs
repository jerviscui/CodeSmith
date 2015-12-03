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
    public partial class TfclosedwallfacilitiesAppService 
    {
        public TfclosedwallfacilitiesAppService(ITfclosedwallfacilitiesRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfclosedwallfacilitiesDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfclosedwallfacilitiesDto input)
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
        protected override IQueryable<Tfclosedwallfacilities> CreateQueryable(GetTfclosedwallfacilitiesPageInput input)
        {
            IQueryable<Tfclosedwallfacilities> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LocationType !=null, t => t.LocationType == input.LocationType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.EnterLength !=null, t => t.EnterLength == input.EnterLength).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.FirePiping !=null, t => t.FirePiping == input.FirePiping).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.BeamTube !=null, t => t.BeamTube == input.BeamTube).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
