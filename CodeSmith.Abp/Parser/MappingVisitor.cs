using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace CodeSmith.Abp.Parser
{
    public class MappingVisitor : DepthFirstAstVisitor<object, object>
    {
        public MappingVisitor()
        {
            MappingBaseType = "EntityTypeConfiguration";
        }

        public string MappingBaseType { get; set; }
        public ParsedEntity ParsedEntity { get; set; }

        public override object VisitTypeDeclaration(TypeDeclaration typeDeclaration, object data)
        {
            var baseType = typeDeclaration.BaseTypes.OfType<MemberType>().FirstOrDefault();
            if (baseType == null || baseType.MemberName != MappingBaseType)
                return base.VisitTypeDeclaration(typeDeclaration, data);

            var entity = baseType.TypeArguments.OfType<MemberType>().FirstOrDefault();
            if (entity == null)
                return base.VisitTypeDeclaration(typeDeclaration, data);

            if (ParsedEntity == null)
                ParsedEntity = new ParsedEntity();

            ParsedEntity.EntityClass = entity.MemberName;
            ParsedEntity.MappingClass = typeDeclaration.Name;

            return base.VisitTypeDeclaration(typeDeclaration, ParsedEntity);
        }

        public override object VisitInvocationExpression(InvocationExpression invocationExpression, object data)
        {
            if (data == null)
                return base.VisitInvocationExpression(invocationExpression, null);

            // visit all the methods
            var identifier = invocationExpression.Target.Children.OfType<Identifier>().FirstOrDefault();
            string methodName = identifier == null ? string.Empty : identifier.Name;

            // the different types of incoming data, helps us know what we're parsing
            var parsedEntity = data as ParsedEntity;
            var parsedProperty = data as ParsedProperty;
            var parsedRelationship = data as ParsedRelationship;

            switch (methodName)
            {
                case "ToTable":
                    var tableNameExpression = invocationExpression.Arguments
                        .OfType<PrimitiveExpression>()
                        .ToArray();

                    string tableName = null;
                    string tableSchema = null;

                    if (tableNameExpression.Length >= 1)
                        tableName = tableNameExpression[0].Value.ToString();
                    if (tableNameExpression.Length >= 2)
                        tableSchema = tableNameExpression[1].Value.ToString();

                    // ToTable is either Entity -> Table map or Many to Many map
                    if (parsedEntity != null)
                    {
                        // when data is ParsedEntity, entity map
                        parsedEntity.TableName = tableName;
                        parsedEntity.TableSchema = tableSchema;
                    }
                    else if (parsedRelationship != null)
                    {
                        // when data is ParsedRelationship, many to many map
                        parsedRelationship.JoinTable = tableName;
                        parsedRelationship.JoinSchema = tableSchema;
                    }
                    break;
                case "HasColumnName":
                    var columnNameExpression = invocationExpression.Arguments
                        .OfType<PrimitiveExpression>()
                        .FirstOrDefault();

                    if (columnNameExpression == null)
                        break;

                    // property to column map start.
                    string columnName = columnNameExpression.Value.ToString();
                    var property = new ParsedProperty { ColumnName = columnName };
                    ParsedEntity.Properties.Add(property);

                    //only have column info at this point. pass data to get property name.
                    return base.VisitInvocationExpression(invocationExpression, property);
                case "Property":
                    var propertyExpression = invocationExpression.Arguments
                        .OfType<LambdaExpression>()
                        .FirstOrDefault();

                    if (parsedProperty == null || propertyExpression == null)
                        break;

                    // ParsedProperty is passed in as data from HasColumnName, add property name
                    var propertyBodyExpression = propertyExpression.Body as MemberReferenceExpression;
                    if (propertyBodyExpression != null)
                        parsedProperty.PropertyName = propertyBodyExpression.MemberName;

                    break;
                case "Map":
                    // start a new Many to Many relationship
                    var mapRelation = new ParsedRelationship();
                    ParsedEntity.Relationships.Add(mapRelation);
                    // pass to child nodes to fill in data
                    return base.VisitInvocationExpression(invocationExpression, mapRelation);
                case "HasForeignKey":
                    var foreignExpression = invocationExpression.Arguments
                        .OfType<LambdaExpression>()
                        .FirstOrDefault();

                    if (foreignExpression == null)
                        break;

                    // when only 1 fkey, body will be member ref
                    if (foreignExpression.Body is MemberReferenceExpression)
                    {
                        var foreignBodyExpression = foreignExpression.Body as MemberReferenceExpression;
                        // start a new relationship
                        var foreignRelation = new ParsedRelationship();
                        ParsedEntity.Relationships.Add(foreignRelation);

                        foreignRelation.ThisProperties.Add(foreignBodyExpression.MemberName);
                        // pass as data for child nodes to fill in data
                        return base.VisitInvocationExpression(invocationExpression, foreignRelation);
                    }
                    // when more then 1 fkey, body will be an anonymous type
                    if (foreignExpression.Body is AnonymousTypeCreateExpression)
                    {
                        var foreignBodyExpression = foreignExpression.Body as AnonymousTypeCreateExpression;
                        var foreignRelation = new ParsedRelationship();
                        ParsedEntity.Relationships.Add(foreignRelation);

                        var properties = foreignBodyExpression.Children
                            .OfType<MemberReferenceExpression>()
                            .Select(m => m.MemberName);

                        foreignRelation.ThisProperties.AddRange(properties);

                        return base.VisitInvocationExpression(invocationExpression, foreignRelation);
                    }
                    break;
                case "HasMany":
                    var hasManyExpression = invocationExpression.Arguments
                        .OfType<LambdaExpression>()
                        .FirstOrDefault();

                    if (parsedRelationship == null || hasManyExpression == null)
                        break;

                    // filling existing relationship data
                    var hasManyBodyExpression = hasManyExpression.Body as MemberReferenceExpression;
                    if (hasManyBodyExpression != null)
                        parsedRelationship.ThisPropertyName = hasManyBodyExpression.MemberName;

                    break;
                case "WithMany":
                    var withManyExpression = invocationExpression.Arguments
                        .OfType<LambdaExpression>()
                        .FirstOrDefault();

                    if (parsedRelationship == null || withManyExpression == null)
                        break;

                    // filling existing relationship data
                    var withManyBodyExpression = withManyExpression.Body as MemberReferenceExpression;
                    if (withManyBodyExpression != null)
                        parsedRelationship.OtherPropertyName = withManyBodyExpression.MemberName;

                    break;
                case "HasRequired":
                case "HasOptional":
                    var hasExpression = invocationExpression.Arguments
                        .OfType<LambdaExpression>()
                        .FirstOrDefault();

                    if (parsedRelationship == null || hasExpression == null)
                        break;

                    // filling existing relationship data
                    var hasBodyExpression = hasExpression.Body as MemberReferenceExpression;
                    if (hasBodyExpression != null)
                        parsedRelationship.ThisPropertyName = hasBodyExpression.MemberName;

                    break;
                case "MapLeftKey":
                    if (parsedRelationship == null)
                        break;

                    var leftKeyExpression = invocationExpression.Arguments
                        .OfType<PrimitiveExpression>()
                        .Select(e => e.Value.ToString());

                    parsedRelationship.JoinThisColumn.AddRange(leftKeyExpression);
                    break;
                case "MapRightKey":
                    if (parsedRelationship == null)
                        break;

                    var rightKeyExpression = invocationExpression.Arguments
                        .OfType<PrimitiveExpression>()
                        .Select(e => e.Value.ToString());

                    parsedRelationship.JoinOtherColumn.AddRange(rightKeyExpression);
                    break;
            }

            return base.VisitInvocationExpression(invocationExpression, data);
        }
    }
}