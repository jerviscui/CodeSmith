using System;
using System.Linq;
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
            if (Content.Settings.EnumTyps.Any(t => t.FullName == dataObjectBase.FullName))
            {
                property.IsEnum = true;
                property.EnumType = Content.Settings.EnumTyps.First(t => t.FullName == dataObjectBase.FullName).EnumType;

            }
            else
            {
                property.IsEnum = false;
            }
            Console.WriteLine(dataObjectBase.FullName + "test");
            return property;
        }
    }
}
