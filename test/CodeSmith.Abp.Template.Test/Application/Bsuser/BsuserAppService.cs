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
    public partial class BsuserAppService 
    {
        public BsuserAppService(IBsuserRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override long Create(BsuserDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsuserDto input)
        {
             base.Update(input);
        }
        
        public override void Delete(IdInput<long> input)
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
        protected override IQueryable<Bsuser> CreateQueryable(GetBsuserPageInput input)
        {
            IQueryable<Bsuser> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.UserName !=null, t => t.UserName == input.UserName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Account !=null, t => t.Account == input.Account).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.PassWord !=null, t => t.PassWord == input.PassWord).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsAction !=null, t => t.IsAction == input.IsAction).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsAdmin !=null, t => t.IsAdmin == input.IsAdmin).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDeleted !=null, t => t.IsDeleted == input.IsDeleted).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.LastLoginDate.HasValue, t => t.LastLoginDate == input.LastLoginDate.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastRole !=null, t => t.LastRole == input.LastRole).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
