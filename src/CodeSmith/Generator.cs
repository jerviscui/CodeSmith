using System;
using System.IO;
using CodeSmith.Engine;

namespace CodeSmith
{
    public static class Generator
    {
        /// <summary>
        /// 创建“编辑”文件与“生成”文件
        /// </summary>
        /// <typeparam name="TGenerate"></typeparam>
        /// <typeparam name="TEditable"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entityContext"></param>
        /// <param name="codeTemplate"></param>
        /// <param name="getGeneratedNamegFun"></param>
        /// <param name="getEditableNameFun"></param>
        /// <param name="rootDirectory"></param>
        public static void CreateMultipleContextClass<TGenerate, TEditable, TEntity, TProperty>(
            EntityContext<TEntity, TProperty> entityContext, CodeTemplate codeTemplate,
            Func<EntityContext<TEntity, TProperty>, string> getGeneratedNamegFun,
            Func<EntityContext<TEntity, TProperty>, string> getEditableNameFun, string rootDirectory)
            where TGenerate : CodeTemplate, new()
            where TEditable : CodeTemplate, new() where TProperty : Property where TEntity : Entity<TProperty>
        {
            TGenerate generatedClass = codeTemplate.Create<TGenerate>();
            codeTemplate.CopyPropertiesTo(generatedClass);

            TEditable editableClass = codeTemplate.Create<TEditable>();
            codeTemplate.CopyPropertiesTo(editableClass);

            //创建根目录
            rootDirectory = Path.GetFullPath(rootDirectory);
            if (!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            //创建编辑文件
            string editableFile = Path.Combine(rootDirectory, getEditableNameFun(entityContext));
            if (!File.Exists(editableFile))
            {
                editableClass.SetProperty("ContextEntity", entityContext);
                codeTemplate.Response.WriteLine(editableFile);
                editableClass.RenderToFile(editableFile, true);
            }

            //创建生成文件
            string generatedFile = Path.Combine(rootDirectory, getGeneratedNamegFun(entityContext));
            generatedClass.SetProperty("ContextEntity", entityContext);
            codeTemplate.Response.WriteLine(generatedFile);
            generatedClass.RenderToFile(generatedFile, editableFile, true);
        }

        /// <summary>
        /// 创建“编辑”文件与“生成”文件
        /// </summary>
        /// <typeparam name="TGenerate"></typeparam>
        /// <typeparam name="TEditable"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entityContext"></param>
        /// <param name="codeTemplate"></param>
        /// <param name="getGeneratedNamegFun"></param>
        /// <param name="getEditableNameFun"></param>
        /// <param name="rootDirectory"></param>
        public static void CreateMultipleEntityClass<TGenerate, TEditable, TEntity, TProperty>(EntityContext<TEntity, 
            TProperty> entityContext, CodeTemplate codeTemplate, 
            Func<TEntity, string> getGeneratedNamegFun, Func<TEntity, string> getEditableNameFun, string rootDirectory)
            where TGenerate : CodeTemplate, new()
            where TEditable : CodeTemplate, new() 
            where TProperty : Property 
            where TEntity : Entity<TProperty>
        {

            TGenerate generatedClass = codeTemplate.Create<TGenerate>();
            codeTemplate.CopyPropertiesTo(generatedClass);

            TEditable editableClass = codeTemplate.Create<TEditable>();
            codeTemplate.CopyPropertiesTo(editableClass);

            //创建根目录
            rootDirectory = Path.GetFullPath(rootDirectory);
            if (!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            foreach (TEntity entity in entityContext.Entities)
            {
              
                string editableFile = Path.Combine(rootDirectory, getEditableNameFun(entity));
                if (!File.Exists(editableFile))
                {
                    editableClass.SetProperty("Entity", entity);
                    codeTemplate.Response.WriteLine(editableFile);
                    editableClass.RenderToFile(editableFile, true);
                }

                string generatedFile = Path.Combine(rootDirectory, getGeneratedNamegFun(entity));
                generatedClass.SetProperty("Entity", entity);
                codeTemplate.Response.WriteLine(generatedFile);
                generatedClass.RenderToFile(generatedFile, editableFile, true);
            }
        }

        public static void CreateEntityClass<T, TEntity, TProperty>(EntityContext<TEntity, TProperty> entityContext, CodeTemplate codeTemplate,
            Func<TEntity, string> getNamegFun, string rootDirectory, bool overwrite = true) where T : CodeTemplate, new() where TEntity : Entity<TProperty> where TProperty : Property
        {
            T classTemplate = codeTemplate.Create<T>();
            codeTemplate.CopyPropertiesTo(classTemplate);

            //创建根目录
            rootDirectory = Path.GetFullPath(rootDirectory);
            if (!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            foreach (TEntity entity in entityContext.Entities)
            {
                string editableFile = Path.Combine(rootDirectory, getNamegFun(entity));
                classTemplate.SetProperty("Entity", entity);
                if (overwrite)
                {
                    codeTemplate.Response.WriteLine(editableFile);
                    classTemplate.RenderToFile(editableFile, true);
                }
                else
                {
                    if (File.Exists(editableFile)) continue;
                    codeTemplate.Response.WriteLine(editableFile);
                    classTemplate.RenderToFile(editableFile, true);
                }
            }
        }
    }
}
