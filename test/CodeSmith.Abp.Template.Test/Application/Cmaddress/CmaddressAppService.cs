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
    public partial class CmaddressAppService 
    {
        public CmaddressAppService(ICmaddressRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(CmaddressDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(CmaddressDto input)
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
        protected override IQueryable<Cmaddress> CreateQueryable(GetCmaddressPageInput input)
        {
            IQueryable<Cmaddress> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressName !=null, t => t.AddressName == input.AddressName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressCategory !=null, t => t.AddressCategory == input.AddressCategory).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsActive !=null, t => t.IsActive == input.IsActive).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDeleted !=null, t => t.IsDeleted == input.IsDeleted).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsPointClosedWall !=null, t => t.IsPointClosedWall == input.IsPointClosedWall).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsChangeDrainage !=null, t => t.IsChangeDrainage == input.IsChangeDrainage).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsChangeGas !=null, t => t.IsChangeGas == input.IsChangeGas).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DuctType !=null, t => t.DuctType == input.DuctType).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
