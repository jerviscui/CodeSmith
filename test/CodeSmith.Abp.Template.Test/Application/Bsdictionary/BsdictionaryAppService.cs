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
    public partial class BsdictionaryAppService 
    {
        public BsdictionaryAppService(IBsdictionaryRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BsdictionaryDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsdictionaryDto input)
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
        protected override IQueryable<Bsdictionary> CreateQueryable(GetBsdictionaryPageInput input)
        {
            IQueryable<Bsdictionary> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ParentId !=null, t => t.ParentId == input.ParentId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ParentIds !=null, t => t.ParentIds == input.ParentIds).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DictionaryKey !=null, t => t.DictionaryKey == input.DictionaryKey).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DictionaryName !=null, t => t.DictionaryName == input.DictionaryName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DictionaryValue !=null, t => t.DictionaryValue == input.DictionaryValue).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsActive !=null, t => t.IsActive == input.IsActive).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDeleted !=null, t => t.IsDeleted == input.IsDeleted).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsGroup !=null, t => t.IsGroup == input.IsGroup).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CreatorUserId !=null, t => t.CreatorUserId == input.CreatorUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.LastModifierUserId !=null, t => t.LastModifierUserId == input.LastModifierUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.LastModificationTime.HasValue, t => t.LastModificationTime == input.LastModificationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Sort !=null, t => t.Sort == input.Sort).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
