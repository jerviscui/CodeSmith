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
    public partial class TfenclosedwallledgerAppService 
    {
        public TfenclosedwallledgerAppService(ITfenclosedwallledgerRepository repository)
            : base(repository)
        {

        }
        
        #region public
        
        public override int Create(TfenclosedwallledgerDto input)
        {
            return base.Create(input);
        }
        
        public override void Update(TfenclosedwallledgerDto input)
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
        protected override IQueryable<Tfenclosedwallledger> CreateQueryable(GetTfenclosedwallledgerPageInput input)
        {
            IQueryable<Tfenclosedwallledger> queryable = base.CreateQueryable(input);
            
            queryable = queryable.WhereIf(input.Id !=null, t => t.Id == input.Id).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MiningArea !=null, t => t.MiningArea == input.MiningArea).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.EnclosedWallNo !=null, t => t.EnclosedWallNo == input.EnclosedWallNo).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.EnclosedWallName !=null, t => t.EnclosedWallName == input.EnclosedWallName).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Place !=null, t => t.Place == input.Place).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Lithology !=null, t => t.Lithology == input.Lithology).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SupportForm !=null, t => t.SupportForm == input.SupportForm).AsQueryable();
         
            queryable = queryable.WhereIf(input.SealOffDate.HasValue, t => t.SealOffDate == input.SealOffDate.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.SystemStatus !=null, t => t.SystemStatus == input.SystemStatus).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.GoafStatus !=null, t => t.GoafStatus == input.GoafStatus).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.TunnelLlength !=null, t => t.TunnelLlength == input.TunnelLlength).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.MaterialQuality !=null, t => t.MaterialQuality == input.MaterialQuality).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Thickness !=null, t => t.Thickness == input.Thickness).AsQueryable();
         
            queryable = queryable.WhereIf(input.CreationTime.HasValue, t => t.CreationTime == input.CreationTime.Value).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.IsClose !=null, t => t.IsClose == input.IsClose).AsQueryable();
         
            
            queryable = queryable.WhereIf(input.Remarks !=null, t => t.Remarks == input.Remarks).AsQueryable();
         
            return queryable;
        }
        
        #endregion
    }
}
