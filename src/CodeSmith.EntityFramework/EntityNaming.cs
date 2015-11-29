namespace CodeSmith.EntityFramework
{
    /// <summary>
    /// 实体命名规则
    /// </summary>
    public enum EntityNaming
    {
        /// <summary>
        /// 保留
        /// </summary>
        Preserve = 0,
        /// <summary>
        /// 复数
        /// </summary>       
        Plural = 1,
        /// <summary>
        /// 单数
        /// </summary>
        Singular = 2
    }
}