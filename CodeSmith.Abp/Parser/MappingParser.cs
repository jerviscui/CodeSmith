using System.Diagnostics;
using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace CodeSmith.Abp.Parser
{
    public static class MappingParser
    {
        public static ParsedEntity Parse(string mappingFile)
        {
            if (string.IsNullOrEmpty(mappingFile) || !File.Exists(mappingFile))
                return null;

            var parser = new CSharpParser();
            SyntaxTree syntaxTree;

            using (var stream = File.OpenText(mappingFile))
                syntaxTree = parser.Parse(stream, mappingFile);

            var visitor = new MappingVisitor();

            visitor.VisitSyntaxTree(syntaxTree, null);
            var parsedEntity = visitor.ParsedEntity;

            if (parsedEntity != null)
                Debug.WriteLine("Parsed Mapping File: '{0}'; Properties: {1}; Relationships: {2}",
                    Path.GetFileName(mappingFile),
                    parsedEntity.Properties.Count,
                    parsedEntity.Relationships.Count);

            return parsedEntity;
        }
    }
}