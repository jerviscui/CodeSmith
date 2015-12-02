using System.Collections.Generic;
using System.Linq;
using CodeSmith.Core.Extensions;
using SchemaExplorer;
// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class AbpEntityTemplate : EntityTemplate<AbpEntity, AbpEntityProperty, AbpEntityPropertyTemplate>
    {
        public AbpEntityTemplate(TemplateContent content)
            : base(content, new AbpEntityPropertyTemplate(content))
        {

        }

        public override AbpEntity GetAndCreate(TableSchema tableSchema)
        {
            AbpEntity abpEntity = base.GetAndCreate(tableSchema);

            abpEntity.PrimaryKey =
                abpEntity.Properties.FirstOrDefault(
                    t =>
                        (t.IsPrimaryKey.HasValue && t.IsPrimaryKey.Value) && t.IsIdentitySpecified);
            if (abpEntity.PrimaryKey == null) return abpEntity;

            string output = " : IEntity<{0}>".FormatWith(abpEntity.PrimaryKey.SystemType.ToType());

            List<string> columnNames = abpEntity.Properties.Select(t => t.PropertyName).ToList();

            if (columnNames.Any(t => t == "CreatorUserId")
                && columnNames.Any(t => t == "CreationTime")
                && columnNames.Any(t => t == "LastModificationTime")
                && columnNames.Any(t => t == "LastModifierUserId"))
                output += ",IAudited";

            if (columnNames.Any(t => t == "CreatorUserId")
                && columnNames.Any(t => t == "CreationTime"))
                output += ",ICreationAudited";

            if (columnNames.Any(t => t == "DeleterUserId")
                && columnNames.Any(t => t == "DeletionTime")
                && columnNames.Any(t => t == "IsDeleted"))
                output += ",IDeletionAudited";

            if (columnNames.Any(t => t == "CreatorUserId")
                && columnNames.Any(t => t == "CreationTime")
                && columnNames.Any(t => t == "LastModificationTime")
                && columnNames.Any(t => t == "LastModifierUserId")
                && columnNames.Any(t => t == "DeleterUserId")
                && columnNames.Any(t => t == "DeletionTime"))
                output += ",IFullAudited";

            if (columnNames.Any(t => t == "CreationTime"))
                output += ",IHasCreationTime";

            if (columnNames.Any(t => t == "LastModificationTime")
                && columnNames.Any(t => t == "LastModifierUserId"))
                output += ",IModificationAudited";

            if (columnNames.Any(t => t == "TenantId"))
            {
                output += ",IMayHaveTenant";
            }

            if (columnNames.Any(t => t == "TenantId"))
                output += ",IMustHaveTenant";

            if (columnNames.Any(t => t == "IsActive"))
                output += ",IPassivable";

            if (columnNames.Any(t => t == "IsDeleted"))
                output += ",ISoftDelete";

            abpEntity.Inherited = output;
            abpEntity.Description = tableSchema.Description.IsNullOrWhiteSpace() ? tableSchema.FullName : tableSchema.Description;

            return abpEntity;
        }
    }
}
