using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace CodeSmith.Abp.Parser
{
    public class ContextVisitor : DepthFirstAstVisitor<object, object>
    {
        public ContextVisitor()
        {
            ContextBaseType = "DbContext";
            DataSetTypes = new HashSet<string> {"DbSet", "IDbSet"};
        }

        public string ContextBaseType { get; set; }
        public HashSet<string> DataSetTypes { get; set; }

        public ParsedContext ParsedContext { get; set; }

        public override object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data)
        {
            var baseType = typeDeclaration.BaseTypes
                .OfType<MemberType>()
                .FirstOrDefault();

            // warning: if inherited from custom base type, this will break
            // anyway to improve this?
            if (baseType == null || baseType.MemberName != ContextBaseType)
                return base.VisitTypeDeclaration(typeDeclaration, data);

            if (ParsedContext == null)
                ParsedContext = new ParsedContext();

            ParsedContext.ContextClass = typeDeclaration.Name;

            return base.VisitTypeDeclaration(typeDeclaration, ParsedContext);
        }

        public override object VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration, object data)
        {
            if (data == null)
                return base.VisitPropertyDeclaration(propertyDeclaration, null);

            // look for property to return generic DbSet type
            var memberType = propertyDeclaration.ReturnType as MemberType;
            if (memberType == null || !DataSetTypes.Contains(memberType.MemberName))
                return base.VisitPropertyDeclaration(propertyDeclaration, data);

            // get the first generic type
            var entityType = memberType.TypeArguments
                .OfType<MemberType>()
                .FirstOrDefault();

            if (entityType == null)
                return base.VisitPropertyDeclaration(propertyDeclaration, data);

            var entitySet = new ParsedEntitySet
            {
                EntityClass = entityType.MemberName,
                ContextProperty = propertyDeclaration.Name
            };

            ParsedContext.Properties.Add(entitySet);

            return base.VisitPropertyDeclaration(propertyDeclaration, data);
        }
    }
}