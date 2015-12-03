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
    public partial class BsroleAppService 
    {
        public BsroleAppService(IBsroleRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BsroleDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsroleDto input)
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
        protected override IQueryable<Bsrole> CreateQueryable(GetBsrolePageInput input)
        {
            IQueryable<Bsrole> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.RoleName !=null, t => t.RoleName == input.RoleName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDeleted !=null, t => t.IsDeleted == input.IsDeleted).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CreatorUserId !=null, t => t.CreatorUserId == input.CreatorUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastModifierUserId !=null, t => t.LastModifierUserId == input.LastModifierUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.LastModificationTime.HasValue, t => t.LastModificationTime == input.LastModificationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
