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
    public partial class TfkeyheadMap
        : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<Tfkeyhead>
    {
        public TfkeyheadMap()
        {
            // table
            ToTable("tfkeyhead");

            // keys
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Id)
                .HasColumnName("Id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
            Property(t => t.Address)
                .HasColumnName("Address")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.SupportingMethod)
                .HasColumnName("SupportingMethod")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.SectionDescription)
                .HasColumnName("SectionDescription")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.EyeNumber)
                .HasColumnName("EyeNumber")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.DeepEye)
                .HasColumnName("DeepEye")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.MouthGas)
                .HasColumnName("MouthGas")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.GasAfterGun)
                .HasColumnName("GasAfterGun")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.AllowFootage)
                .HasColumnName("AllowFootage")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.ExcessSize)
                .HasColumnName("ExcessSize")
                .HasMaxLength(765)
                .IsOptional();
            Property(t => t.ImgUrl)
                .HasColumnName("ImgUrl")
                .IsOptional();
            Property(t => t.CollectionTime)
                .HasColumnName("CollectionTime")
                .IsRequired();
            Property(t => t.CreationTime)
                .HasColumnName("CreationTime")
                .IsRequired();
            Property(t => t.Remarks)
                .HasColumnName("Remarks")
                .IsOptional();

            // Relationships

            InitializeMapping();
        }
    }
}
