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
    public partial class Cmaddress : IEntity<int>,IHasCreationTime,IPassivable,ISoftDelete
    {
        public Cmaddress()
        {
            AddressCmaddressconfigs = new List<Cmaddressconfig>();
            AddressTfclosedwallfacilities = new List<Tfclosedwallfacilities>();
            AddressTfdrillings = new List<Tfdrilling>();
            AddressTfgasdrainages = new List<Tfgasdrainage>();
            AddressTfmonitoranalogs = new List<Tfmonitoranalog>();
            AddressTfpointclosedwalls = new List<Tfpointclosedwall>();
            AddressTfproofreads = new List<Tfproofread>();
            AddressWsclosedwallfeatures = new List<Wsclosedwallfeature>();
            AddressWsgas = new List<Wsgas>();
            AddressZtporeplates = new List<Ztporeplate>();
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
        public string AddressName { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public int AddressCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool IsActive { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool IsPointClosedWall { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool IsChangeDrainage { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool IsChangeGas { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public int DuctType { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public System.DateTime CreationTime { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public System.DateTime CollectionTime { get; set; }

        public virtual ICollection<Cmaddressconfig> AddressCmaddressconfigs { get; set; }
        public virtual ICollection<Tfclosedwallfacilities> AddressTfclosedwallfacilities { get; set; }
        public virtual ICollection<Tfdrilling> AddressTfdrillings { get; set; }
        public virtual ICollection<Tfgasdrainage> AddressTfgasdrainages { get; set; }
        public virtual ICollection<Tfmonitoranalog> AddressTfmonitoranalogs { get; set; }
        public virtual ICollection<Tfpointclosedwall> AddressTfpointclosedwalls { get; set; }
        public virtual ICollection<Tfproofread> AddressTfproofreads { get; set; }
        public virtual ICollection<Wsclosedwallfeature> AddressWsclosedwallfeatures { get; set; }
        public virtual ICollection<Wsgas> AddressWsgas { get; set; }
        public virtual ICollection<Ztporeplate> AddressZtporeplates { get; set; }
    }
}