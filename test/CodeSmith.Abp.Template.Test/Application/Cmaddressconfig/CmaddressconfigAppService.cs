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
    public partial class CmaddressconfigAppService 
    {
        public CmaddressconfigAppService(ICmaddressconfigRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(CmaddressconfigDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(CmaddressconfigDto input)
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
        protected override IQueryable<Cmaddressconfig> CreateQueryable(GetCmaddressconfigPageInput input)
        {
            IQueryable<Cmaddressconfig> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SensorId !=null, t => t.SensorId == input.SensorId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SysType !=null, t => t.SysType == input.SysType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ObjectType !=null, t => t.ObjectType == input.ObjectType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Code !=null, t => t.Code == input.Code).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
