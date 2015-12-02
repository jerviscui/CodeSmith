// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class AbpEntity : Entity<AbpEntityProperty>
    {
        /// <summary>
        /// 需要继承Abp 审计属性的接口
        /// </summary>
        public string Inherited { get; set; }

        /// <summary>
        /// 主键属性
        /// </summary>
        public AbpEntityProperty PrimaryKey { get; set; }
    }
}
