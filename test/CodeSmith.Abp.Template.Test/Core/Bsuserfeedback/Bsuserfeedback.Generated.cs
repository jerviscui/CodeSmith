﻿//------------------------------------------------------------------------------
// <autogenerated>
//     此代码是由 CodeSmith 模板生成的。
//
//     请不要对该文件内容进行修改。如果重新生成代码，文件内容将被覆盖。
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

// ReSharper disable once CheckNamespace
namespace CodeSmith.Abp.Template.Test.Core
{
    public partial class Bsuserfeedback : IEntity<int>,ICreationAudited,IHasCreationTime,ISoftDelete
    {
        public Bsuserfeedback()
        {
        }
        
        public bool IsTransient()
        {
            return EqualityComparer<int>.Default.Equals(this.Id, default(int));
        }

        /// <summary>
        /// 
        /// </summary>    
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string Contents { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public int FeedbackType { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string FeedbackImg { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public long? CreatorUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public System.DateTime CreationTime { get; set; }

        public virtual Bsuser CreatorUserBsuser { get; set; }
    }
}