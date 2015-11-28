using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CodeSmith.Engine;

namespace CodeSmith.EntityFramework
{
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

        public TableNaming TableNaming { get; set; }

        public EntityNaming EntityNaming { get; set; }

        public RelationshipNaming RelationshipNaming { get; set; }

        public ContextNaming ContextNaming { get; set; }

        public List<string> CleanExpressions { get; set; }

        public Dictionary<string, string> RenameRules { get; set; }

        public bool IncludeViews { get; set; }


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

        public string RelationshipName(string name)
        {
            if (RelationshipNaming == RelationshipNaming.None)
                return name;

            if (RelationshipNaming == RelationshipNaming.ListSuffix)
                return name + "List";

            return StringUtil.ToPlural(name);
        }

        public string ContextName(string name)
        {
            if (ContextNaming == ContextNaming.Preserve)
                return name;

            if (ContextNaming == ContextNaming.TableSuffix)
                return name + "Table";

            return StringUtil.ToPlural(name);
        }

        public string EntityName(string name)
        {
            if (TableNaming != TableNaming.Plural && EntityNaming == EntityNaming.Plural)
                name = StringUtil.ToPlural(name);
            else if (TableNaming != TableNaming.Singular && EntityNaming == EntityNaming.Singular)
                name = StringUtil.ToSingular(name);

            return name;
        }
    }
}