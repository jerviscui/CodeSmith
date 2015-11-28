using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CodeSmith.Abp.Model;

namespace CodeSmith.Abp.Parser
{
    #region Mapping Parser

    #endregion

    #region Context Parser

    #endregion

    public static class Synchronizer
    {
        public static bool UpdateFromSource(EntityContext generatedContext, string contextDirectory, string mappingDirectory)
        {
            if (generatedContext == null)
                return false;

            // make sure to update the entities before the context
            UpdateFromMapping(generatedContext, mappingDirectory);
            UpdateFromContext(generatedContext, contextDirectory);
            return true;
        }

        private static void UpdateFromContext(EntityContext generatedContext, string contextDirectory)
        {
            if (generatedContext == null
              || contextDirectory == null
              || !Directory.Exists(contextDirectory))
                return;

            // parse context
            ParsedContext parsedContext = null;
            var files = Directory.EnumerateFiles(contextDirectory, "*.Generated.cs").GetEnumerator();
            while (files.MoveNext() && parsedContext == null)
                parsedContext = ContextParser.Parse(files.Current);

            if (parsedContext == null)
                return;

            if (generatedContext.ClassName != parsedContext.ContextClass)
            {
                Debug.WriteLine("Rename Context Class'{0}' to '{1}'.",
                                generatedContext.ClassName,
                                parsedContext.ContextClass);

                generatedContext.ClassName = parsedContext.ContextClass;
            }

            foreach (var parsedProperty in parsedContext.Properties)
            {
                var entity = generatedContext.Entities.ByClass(parsedProperty.EntityClass);
                if (entity == null)
                    continue;


                if (entity.ContextName == parsedProperty.ContextProperty)
                    continue;

                Debug.WriteLine("Rename Context Property'{0}' to '{1}'.",
                                entity.ContextName,
                                parsedProperty.ContextProperty);

                entity.ContextName = parsedProperty.ContextProperty;
            }
        }

        private static void UpdateFromMapping(EntityContext generatedContext, string mappingDirectory)
        {
            if (generatedContext == null
              || mappingDirectory == null
              || !Directory.Exists(mappingDirectory))
                return;

            // parse all mapping files
            var mappingFiles = Directory.EnumerateFiles(mappingDirectory, "*.Generated.cs");
            var parsedEntities = mappingFiles
              .Select(MappingParser.Parse)
              .Where(parsedEntity => parsedEntity != null)
              .ToList();

            var relationshipQueue = new List<Tuple<Entity, ParsedEntity>>();

            // update all entity and property names first because relationships are linked by property names
            foreach (var parsedEntity in parsedEntities)
            {
                // find entity by table name to support renaming entity
                var entity = generatedContext.Entities
                  .ByTable(parsedEntity.TableName, parsedEntity.TableSchema);

                if (entity == null)
                    continue;

                // sync names
                if (entity.MappingName != parsedEntity.MappingClass)
                {
                    Debug.WriteLine("Rename Mapping Class'{0}' to '{1}'.",
                          entity.MappingName,
                          parsedEntity.MappingClass);

                    entity.MappingName = parsedEntity.MappingClass;
                }

                // use rename api to make sure all instances are renamed
                generatedContext.RenameEntity(entity.ClassName, parsedEntity.EntityClass);

                // sync properties
                foreach (var parsedProperty in parsedEntity.Properties)
                {
                    // find property by column name to support property name rename
                    var property = entity.Properties.ByColumn(parsedProperty.ColumnName);
                    if (property == null)
                        continue;

                    // use rename api to make sure all instances are renamed
                    generatedContext.RenameProperty(
                      entity.ClassName,
                      property.PropertyName,
                      parsedProperty.PropertyName);
                }

                // save relationship for later processing
                var item = new Tuple<Entity, ParsedEntity>(entity, parsedEntity);
                relationshipQueue.Add(item);
            }

            // update relationships last
            foreach (var tuple in relationshipQueue)
                UpdateRelationships(generatedContext, tuple.Item1, tuple.Item2);
        }

        private static void UpdateRelationships(EntityContext generatedContext, Entity entity, ParsedEntity parsedEntity)
        {
            // sync relationships
            foreach (var parsedRelationship in parsedEntity.Relationships.Where(r => r.JoinTable == null))
            {
                var parsedProperties = parsedRelationship.ThisProperties;
                var relationship = entity.Relationships
                    .Where(r => !r.IsManyToMany)
                    .FirstOrDefault(r => r.ThisProperties.Except(parsedProperties).Count() == 0);

                if (relationship == null)
                    continue;

                bool isThisSame = relationship.ThisPropertyName == parsedRelationship.ThisPropertyName;
                bool isOtherSame = relationship.OtherPropertyName == parsedRelationship.OtherPropertyName;

                if (isThisSame && isOtherSame)
                    continue;

                if (!isThisSame)
                {
                    Debug.WriteLine("Rename Relationship Property '{0}.{1}' to '{0}.{2}'.",
                          relationship.ThisEntity,
                          relationship.ThisPropertyName,
                          parsedRelationship.ThisPropertyName);

                    relationship.ThisPropertyName = parsedRelationship.ThisPropertyName;
                }
                if (!isOtherSame)
                {
                    Debug.WriteLine("Rename Relationship Property '{0}.{1}' to '{0}.{2}'.",
                          relationship.OtherEntity,
                          relationship.OtherPropertyName,
                          parsedRelationship.OtherPropertyName);

                    relationship.OtherPropertyName = parsedRelationship.OtherPropertyName;
                }

                // sync other relationship
                var otherEntity = generatedContext.Entities.ByClass(relationship.OtherEntity);
                if (otherEntity == null)
                    continue;

                var otherRelationship = otherEntity.Relationships.ByName(relationship.RelationshipName);
                if (otherRelationship == null)
                    continue;

                otherRelationship.ThisPropertyName = relationship.OtherPropertyName;
                otherRelationship.OtherPropertyName = relationship.ThisPropertyName;
            }

            // sync many to many
            foreach (var parsedRelationship in parsedEntity.Relationships.Where(r => r.JoinTable != null))
            {
                var joinThisColumn = parsedRelationship.JoinThisColumn;
                var joinOtherColumn = parsedRelationship.JoinOtherColumn;

                var relationship = entity.Relationships
                  .Where(r => r.IsManyToMany)
                  .FirstOrDefault(r =>
                                  r.JoinThisColumn.Except(joinThisColumn).Count() == 0 &&
                                  r.JoinOtherColumn.Except(joinOtherColumn).Count() == 0 &&
                                  r.JoinTable == parsedRelationship.JoinTable &&
                                  r.JoinSchema == parsedRelationship.JoinSchema);

                if (relationship == null)
                    continue;


                bool isThisSame = relationship.ThisPropertyName == parsedRelationship.ThisPropertyName;
                bool isOtherSame = relationship.OtherPropertyName == parsedRelationship.OtherPropertyName;

                if (isThisSame && isOtherSame)
                    continue;

                if (!isThisSame)
                {
                    Debug.WriteLine("Rename Relationship Property '{0}.{1}' to '{0}.{2}'.",
                          relationship.ThisEntity,
                          relationship.ThisPropertyName,
                          parsedRelationship.ThisPropertyName);

                    relationship.ThisPropertyName = parsedRelationship.ThisPropertyName;
                }
                if (!isOtherSame)
                {
                    Debug.WriteLine("Rename Relationship Property '{0}.{1}' to '{0}.{2}'.",
                          relationship.OtherEntity,
                          relationship.OtherPropertyName,
                          parsedRelationship.OtherPropertyName);

                    relationship.OtherPropertyName = parsedRelationship.OtherPropertyName;
                }

                // sync other relationship
                var otherEntity = generatedContext.Entities.ByClass(relationship.OtherEntity);
                if (otherEntity == null)
                    continue;

                var otherRelationship = otherEntity.Relationships.ByName(relationship.RelationshipName);
                if (otherRelationship == null)
                    continue;

                otherRelationship.ThisPropertyName = relationship.OtherPropertyName;
                otherRelationship.OtherPropertyName = relationship.ThisPropertyName;
            }
        }
    }
}
