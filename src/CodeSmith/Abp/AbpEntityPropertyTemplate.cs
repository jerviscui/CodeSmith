using System;
using CodeSmith.Core.Extensions;
using SchemaExplorer;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class AbpEntityPropertyTemplate : PropertyTemplate<AbpEntity, AbpEntityProperty>
    {
        public AbpEntityPropertyTemplate(TemplateContent content)
            : base(content)
        {

        }

        public override AbpEntityProperty GetAndCreate(DataObjectBase dataObjectBase)
        {
            AbpEntityProperty property = base.GetAndCreate(dataObjectBase);
            string key = "{0}{1}".FormatWith(Entity.TableName, dataObjectBase.Name);
            if (Content.Settings.EnumTyps.ContainsKey(key))
            {
                property.IsEnum = true;
                property.EnumType = Content.Settings.EnumTyps[key];

            }
            else
            {
                property.IsEnum = false;
            }
            Console.WriteLine(dataObjectBase.FullName+"test");
            return property;
        }
    }
}
