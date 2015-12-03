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
    public partial class CmdictionaryconfigAppService 
    {
        public CmdictionaryconfigAppService(ICmdictionaryconfigRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(CmdictionaryconfigDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(CmdictionaryconfigDto input)
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
        protected override IQueryable<Cmdictionaryconfig> CreateQueryable(GetCmdictionaryconfigPageInput input)
        {
            IQueryable<Cmdictionaryconfig> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DictionaryKey !=null, t => t.DictionaryKey == input.DictionaryKey).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.DictionaryValue !=null, t => t.DictionaryValue == input.DictionaryValue).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
