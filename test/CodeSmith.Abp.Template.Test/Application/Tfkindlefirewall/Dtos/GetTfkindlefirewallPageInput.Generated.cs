﻿//------------------------------------------------------------------------------
// <autogenerated>
//     此代码是由 CodeSmith 模板生成的。
//
//     做不该文件的修改内容。更改此
//     如果重新生成代码，文件将会丢失。
// </autogenerated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

// ReSharper disable once CheckNamespace
namespace CodeSmith.Abp.Template.Test.Application
{
    public partial class GetTfkindlefirewallPageInput : DefaultPagedResultRequest
    {
        
        /// <summary>
        /// 
        /// </summary>    
        public int? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public int? EnclosedWallLedgerId { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? GasConcentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? COConcentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? CO2Concentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? Temperature { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? O2Concentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? OutsideGasConcentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? OutsideCO2Concentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? OutsideCOConcentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? OutsideTConcentration { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? DifferentialPressure { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public decimal? WaterTemperature { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool? IsSensor { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool? IsBeamTube { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool? IsCOSensor { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public bool? IsDrainage { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public System.DateTime? CreationTime { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public System.DateTime? CollectionTime { get; set; }
        /// <summary>
        /// 
        /// </summary>    
        public string Remarks { get; set; }
    }
}

