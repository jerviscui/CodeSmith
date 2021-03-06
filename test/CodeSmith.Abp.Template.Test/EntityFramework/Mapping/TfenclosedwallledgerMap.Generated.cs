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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Abp.EntityFramework;
using CodeSmith.Abp.Template.Test.Core;

// ReSharper disable once CheckNamespace
namespace CodeSmith.Abp.Template.Test.EntityFramework
{
    public partial class TfenclosedwallledgerMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Tfenclosedwallledger>
    {
        public TfenclosedwallledgerMap()
        {
            // table
            ToTable("tfenclosedwallledger");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.MiningArea)
                .HasColumnName("MiningArea")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.EnclosedWallNo)
                .HasColumnName("EnclosedWallNo")
                .IsRequired();
            Property(t => t.EnclosedWallName)
                .HasColumnName("EnclosedWallName")
                .HasMaxLength(765)
                .IsRequired();
            Property(t => t.Place)
                .HasColumnName("Place")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.Lithology)
                .HasColumnName("Lithology")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.SupportForm)
                .HasColumnName("SupportForm")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.SealOffDate)
                .HasColumnName("SealOffDate")
                .IsOptional();
            Property(t => t.SystemStatus)
                .HasColumnName("SystemStatus")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.GoafStatus)
                .HasColumnName("GoafStatus")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.TunnelLlength)
                .HasColumnName("TunnelLlength")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.MaterialQuality)
                .HasColumnName("MaterialQuality")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.Thickness)
                .HasColumnName("Thickness")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.CreationTime)
                .HasColumnName("CreationTime")
                .IsRequired();
            Property(t => t.IsClose)
                .HasColumnName("IsClose")
                .IsRequired();
            Property(t => t.Remarks)
                .HasColumnName("Remarks")
                .HasMaxLength(765)
                .IsOptional();

            // Relationships

            InitializeMapping();
        }
    }
}
