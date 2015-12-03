using Abp.Application.Services;

namespace CodeSmith.Abp.Template.Test.Application
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ServiceBase : ApplicationService
    {
        protected ServiceBase()
        {

            LocalizationSourceName = "123";
        }
    }
}