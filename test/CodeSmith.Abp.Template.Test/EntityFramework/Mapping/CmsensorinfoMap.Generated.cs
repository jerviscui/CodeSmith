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
    public partial class CmsensorinfoMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cmsensorinfo>
    {
        public CmsensorinfoMap()
        {
            // table
            ToTable("cmsensorinfo");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.SysType)
                .HasColumnName("SysType")
                .IsRequired();
            Property(t => t.SysTypeName)
                .HasColumnName("SysTypeName")
                .HasMaxLength(150)
                .IsOptional();
            Property(t => t.ObjectType)
                .HasColumnName("ObjectType")
                .IsRequired();
            Property(t => t.ObjectTypeName)
                .HasColumnName("ObjectTypeName")
                .HasMaxLength(150)
                .IsOptional();
            Property(t => t.ObjectName)
                .HasColumnName("ObjectName")
                .HasMaxLength(600)
                .IsOptional();
            Property(t => t.Code)
                .HasColumnName("Code")
                .HasMaxLength(150)
                .IsRequired();
            Property(t => t.IsUse)
                .HasColumnName("IsUse")
                .IsRequired();
            Property(t => t.CreationTime)
                .HasColumnName("CreationTime")
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
