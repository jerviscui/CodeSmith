using System.Diagnostics;
using System.IO;
using ICSharpCode.NRefactory.CSharp;

namespace CodeSmith.Abp.Parser
{
    public static class ContextParser
    {
        public static ParsedContext Parse(string contextFile)
        {
            if (string.IsNullOrEmpty(contextFile) || !File.Exists(contextFile))
                return null;

            var parser = new CSharpParser();
            SyntaxTree syntaxTree;

            using (var stream = File.OpenText(contextFile))
                syntaxTree = parser.Parse(stream, contextFile);

            var visitor = new ContextVisitor();

            visitor.VisitSyntaxTree(syntaxTree, null);
            var parsedContext = visitor.ParsedContext;

            if (parsedContext != null)
                Debug.WriteLine("Parsed Context File: '{0}'; Entities: {1}",
                    Path.GetFileName(contextFile),
                    parsedContext.Properties.Count);

            return parsedContext;
        }
    }
}