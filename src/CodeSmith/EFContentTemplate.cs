using System;
using System.Diagnostics;
using System.Linq;
using CodeSmith.Engine;
using CodeSmith.Model;
using SchemaExplorer;

namespace CodeSmith
{
    public class EFContentTemplate : ISingleTemplate<EntityContext, SchemaSelector>
    {
        private readonly TemplateContent _content;

        /// <summary>
        /// 数据库结构处理事件
        /// </summary>
        public event EventHandler<SchemaItemProcessedEventArgs> SchemaItemProcessed;

        public EFContentTemplate(TemplateContent content)
        {
            _content = content;
        }

        public EntityContext Get(SchemaSelector databaseSchema)
        {
            EntityContext entityContext = new EntityContext { DatabaseName = databaseSchema.Database.Name };

            string dataContextName = StringUtil.ToPascalCase(entityContext.DatabaseName) + "Context";
            dataContextName = _content.UniqueNamer.UniqueClassName(dataContextName);

            entityContext.ClassName = dataContextName;

            var tables = databaseSchema.Tables
                .Selected()
                .OrderBy(t => t.SortName)
                .ToTables();

            foreach (TableSchema t in tables)
            {
                if (_content.IsManyToMany(t))
                {
                    Debug.WriteLine("多对多表: " + t.FullName);
                    CreateManyToMany(entityContext, t);
                }
                else
                {
                    Debug.WriteLine("表: " + t.FullName);
                    GetEntity(entityContext, t);
                }

                OnSchemaItemProcessed(t.FullName);
            }

            if (!_content.GeneratorSettings.IncludeViews) return entityContext;
            // 数据库视图生成EF实体
            var views = databaseSchema.Views
                .Selected()
                .OrderBy(t => t.SortName)
                .ToViews();
            foreach (ViewSchema v in views)
            {
                Debug.WriteLine("视图: " + v.FullName);
                GetEntity(entityContext, v);
                OnSchemaItemProcessed(v.FullName);
            }

            return entityContext;
        }

        protected void OnSchemaItemProcessed(string name)
        {
            var handler = SchemaItemProcessed;
            if (handler == null)
                return;

            handler(this, new SchemaItemProcessedEventArgs(name));
        }
    }
}
