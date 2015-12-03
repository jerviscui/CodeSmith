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
    public partial class BsuserfeedbackAppService 
    {
        public BsuserfeedbackAppService(IBsuserfeedbackRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(BsuserfeedbackDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(BsuserfeedbackDto input)
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
        protected override IQueryable<Bsuserfeedback> CreateQueryable(GetBsuserfeedbackPageInput input)
        {
            IQueryable<Bsuserfeedback> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Title !=null, t => t.Title == input.Title).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Contents !=null, t => t.Contents == input.Contents).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.FeedbackType !=null, t => t.FeedbackType == input.FeedbackType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsDeleted !=null, t => t.IsDeleted == input.IsDeleted).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.FeedbackImg !=null, t => t.FeedbackImg == input.FeedbackImg).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.CreatorUserId !=null, t => t.CreatorUserId == input.CreatorUserId).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
