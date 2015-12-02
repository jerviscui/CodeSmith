// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class AbpDbContentTemplate : DbContentTemplate<AbpEntity, AbpEntityProperty, AbpEntityTemplate, AbpEntityPropertyTemplate>
   {
        public AbpDbContentTemplate(TemplateContent content)
            : base(content, new AbpEntityTemplate(content))
        {
        }
   }
}
