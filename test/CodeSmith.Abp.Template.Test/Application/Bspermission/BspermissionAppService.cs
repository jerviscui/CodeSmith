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
    public partial class BspermissionAppService 
    {
        public BspermissionAppService(IBspermissionRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BspermissionDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BspermissionDto input)
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
        protected override IQueryable<Bspermission> CreateQueryable(GetBspermissionPageInput input)
        {
            IQueryable<Bspermission> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PrivilegeMaster !=null, t => t.PrivilegeMaster == input.PrivilegeMaster).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PrivilegeMasterValue !=null, t => t.PrivilegeMasterValue == input.PrivilegeMasterValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PrivilegeAccess !=null, t => t.PrivilegeAccess == input.PrivilegeAccess).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PrivilegeAccessValue !=null, t => t.PrivilegeAccessValue == input.PrivilegeAccessValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PrivilegeOperation !=null, t => t.PrivilegeOperation == input.PrivilegeOperation).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
