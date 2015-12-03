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
    public partial class TfproofreadAppService 
    {
        public TfproofreadAppService(ITfproofreadRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfproofreadDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfproofreadDto input)
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
        protected override IQueryable<Tfproofread> CreateQueryable(GetTfproofreadPageInput input)
        {
            IQueryable<Tfproofread> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.AddressId !=null, t => t.AddressId == input.AddressId).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.ProofType !=null, t => t.ProofType == input.ProofType).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.K !=null, t => t.K == input.K).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Ch4 !=null, t => t.Ch4 == input.Ch4).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.H !=null, t => t.H == input.H).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.T !=null, t => t.T == input.T).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Kpa !=null, t => t.Kpa == input.Kpa).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Qh !=null, t => t.Qh == input.Qh).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Qb !=null, t => t.Qb == input.Qb).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            queryable = queryable.WhereIf(input.CollectionTime.HasValue, t => t.CollectionTime == input.CollectionTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
