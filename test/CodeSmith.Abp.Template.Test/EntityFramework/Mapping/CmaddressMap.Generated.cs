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
    public partial class CmaddressMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Cmaddress>
    {
        public CmaddressMap()
        {
            // table
            ToTable("cmaddress");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.AddressName)
                .HasColumnName("AddressName")
                .HasMaxLength(150)
                .IsRequired();
            Property(t => t.AddressCategory)
                .HasColumnName("AddressCategory")
                .IsRequired();
            Property(t => t.IsActive)
                .HasColumnName("IsActive")
                .IsRequired();
            Property(t => t.IsDeleted)
                .HasColumnName("IsDeleted")
                .IsRequired();
            Property(t => t.IsPointClosedWall)
                .HasColumnName("IsPointClosedWall")
                .IsRequired();
            Property(t => t.IsChangeDrainage)
                .HasColumnName("IsChangeDrainage")
                .IsRequired();
            Property(t => t.IsChangeGas)
                .HasColumnName("IsChangeGas")
                .IsRequired();
            Property(t => t.DuctType)
                .HasColumnName("DuctType")
                .IsRequired();
            Property(t => t.CreationTime)
                .HasColumnName("CreationTime")
                .IsRequired();
            Property(t => t.CollectionTime)
                .HasColumnName("CollectionTime")
                .IsRequired();

            // Relationships

            InitializeMapping();
        }
    }
}
