// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class AbpEntityProperty : Property
    {
        /// <summary>
        /// 是否枚举
        /// </summary>
        public bool IsEnum { get; set; }

        /// <summary>
        /// 枚举类型
        /// </summary>
        public string EnumType { get; set; }
    }
}
