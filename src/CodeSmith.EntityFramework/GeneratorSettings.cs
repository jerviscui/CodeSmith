using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeSmith.Engine;

namespace CodeSmith.EntityFramework
{
    /// <summary>
    /// 生成代码配置
    /// </summary>
    public class GeneratorSettings
    {
        public GeneratorSettings()
        {
            RelationshipNaming = RelationshipNaming.ListSuffix;
            EntityNaming = EntityNaming.Singular;
            TableNaming = TableNaming.Singular;
            CleanExpressions = new List<string> { @"^\d+" };
            RenameRules = new Dictionary<string, string>();
        }

        /// <summary>
        /// 数据库表名格式
        /// </summary>
        public TableNaming TableNaming { get; set; }
        /// <summary>
        /// 实体类名格式
        /// </summary>
        public EntityNaming EntityNaming { get; set; }
        /// <summary>
        /// 关系类名格式
        /// </summary>
        public RelationshipNaming RelationshipNaming { get; set; }
        /// <summary>
        /// 上下文类名格式
        /// </summary>
        public ContextNaming ContextNaming { get; set; }

        /// <summary>
        /// 清除表名正则表达式
        /// </summary>
        public List<string> CleanExpressions { get; set; }

        /// <summary>
        /// 需要重新命名的表名规则
        /// </summary>

        public Dictionary<string, string> RenameRules { get; set; }
       
        /// <summary>
        /// 生成代码是否包含视图
        /// </summary>
        public bool IncludeViews { get; set; }

        /// <summary>
        /// 数据库字段重新命名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string CleanName(string name)
        {
            string rename;
            if (RenameRules.TryGetValue(name, out rename))
                return rename;

            if (CleanExpressions.Count == 0)
                return name;

            foreach (var regex in CleanExpressions.Where(r => !string.IsNullOrEmpty(r)))
                if (Regex.IsMatch(name, regex))
                    return Regex.Replace(name, regex, "");

            return name;
        }

        /// <summary>
        /// 获取关系类名
        /// None:无
        /// ListSuffix:包含“List”后缀
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string RelationshipName(string name)
        {
            if (RelationshipNaming == RelationshipNaming.None)
                return name;

            if (RelationshipNaming == RelationshipNaming.ListSuffix)
                return name + "List";

            return StringUtil.ToPlural(name);
        }

        /// <summary>
        /// 获取上下文类名
        /// TableSuffix:包含“Table”后缀
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string ContextName(string name)
        {
            if (ContextNaming == ContextNaming.Preserve)
                return name;

            if (ContextNaming == ContextNaming.TableSuffix)
                return name + "Table";

            return StringUtil.ToPlural(name);
        }

        /// <summary>
        /// 获取实体类名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string EntityName(string name)
        {
            //表名不为复数格式，实体名为复数格式，将实体名采用复数格式
            if (TableNaming != TableNaming.Plural && EntityNaming == EntityNaming.Plural)
                name = StringUtil.ToPlural(name);
            //表名不为单数格式，实体名为单数格式，将实体名采用复数格式
            else if (TableNaming != TableNaming.Singular && EntityNaming == EntityNaming.Singular)
                name = StringUtil.ToSingular(name);

            return name;
        }
    }
}