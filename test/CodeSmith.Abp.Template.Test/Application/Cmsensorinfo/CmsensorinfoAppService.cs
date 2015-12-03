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
    public partial class CmsensorinfoAppService 
    {
        public CmsensorinfoAppService(ICmsensorinfoRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(CmsensorinfoDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(CmsensorinfoDto input)
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
        protected override IQueryable<Cmsensorinfo> CreateQueryable(GetCmsensorinfoPageInput input)
        {
            IQueryable<Cmsensorinfo> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SysType !=null, t => t.SysType == input.SysType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SysTypeName !=null, t => t.SysTypeName == input.SysTypeName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ObjectType !=null, t => t.ObjectType == input.ObjectType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ObjectTypeName !=null, t => t.ObjectTypeName == input.ObjectTypeName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ObjectName !=null, t => t.ObjectName == input.ObjectName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Code !=null, t => t.Code == input.Code).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsUse !=null, t => t.IsUse == input.IsUse).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
