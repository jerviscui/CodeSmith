using System;
using System.IO;
using CodeSmith.Abp.Model;
using CodeSmith.Engine;

namespace CodeSmith.Abp.Generator
{
    public static class AbpGenerator
    {
        //public static dynamic GetPaht()
        //{
        //    dynamic dictionary = new
        //    {
        //        GetCurrentDirectory = System.IO.Directory.GetCurrentDirectory(),
        //        GetCurrentDirectory = System.IO.Directory.GetCurrentDirectory(),
        //        GetCurrentDirectory = System.IO.Directory.GetCurrentDirectory(),
        //        GetCurrentDirectory = System.IO.Directory.GetCurrentDirectory(),
        //        GetCurrentDirectory = System.IO.Directory.GetCurrentDirectory(),
        //    };
        //    return dictionary;
        //}

        /// <summary>
        /// 创建“编辑”文件与“生成”文件
        /// </summary>
        /// <typeparam name="TGenerate"></typeparam>
        /// <typeparam name="TEditable"></typeparam>
        /// <param name="entityContext"></param>
        /// <param name="codeTemplate"></param>
        /// <param name="getGeneratedNamegFun"></param>
        /// <param name="getEditableNameFun"></param>
        /// <param name="rootDirectory"></param>
        public static void CreateMultipleContextClass<TGenerate, TEditable>(EntityContext entityContext, CodeTemplate codeTemplate, Func<EntityContext, string> getGeneratedNamegFun, Func<EntityContext, string> getEditableNameFun, string rootDirectory)
            where TGenerate : CodeTemplate, new()
            where TEditable : CodeTemplate, new()
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
                editableClass.SetProperty("EntityContext", entityContext);
                codeTemplate.Response.WriteLine(editableFile);
                editableClass.RenderToFile(editableFile, true);
            }

            //创建生成文件
            string generatedFile = Path.Combine(rootDirectory, getGeneratedNamegFun(entityContext));
            generatedClass.SetProperty("EntityContext", entityContext);
            codeTemplate.Response.WriteLine(generatedFile);
            generatedClass.RenderToFile(generatedFile, editableFile, true);
        }

        /// <summary>
        /// 创建“编辑”文件与“生成”文件
        /// </summary>
        /// <typeparam name="TGenerate"></typeparam>
        /// <typeparam name="TEditable"></typeparam>
        /// <param name="entityContext"></param>
        /// <param name="codeTemplate"></param>
        /// <param name="getGeneratedNamegFun"></param>
        /// <param name="getEditableNameFun"></param>
        /// <param name="rootDirectory"></param>
        public static void CreateMultipleEntityClass<TGenerate, TEditable>(EntityContext entityContext, CodeTemplate codeTemplate, Func<Entity, string> getGeneratedNamegFun, Func<Entity, string> getEditableNameFun, string rootDirectory)
            where TGenerate : CodeTemplate, new()
            where TEditable : CodeTemplate, new()
        {
            TGenerate generatedClass = codeTemplate.Create<TGenerate>();
            codeTemplate.CopyPropertiesTo(generatedClass);

            TEditable editableClass = codeTemplate.Create<TEditable>();
            codeTemplate.CopyPropertiesTo(editableClass);

            //创建根目录
            rootDirectory = Path.GetFullPath(rootDirectory);
            if (!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            foreach (Entity entity in entityContext.Entities)
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

        public static void CreateEntityClass<T>(EntityContext entityContext, CodeTemplate codeTemplate,
            Func<Entity, string> getNamegFun, string rootDirectory, bool overwrite = true) where T : CodeTemplate, new()
        {
            T classTemplate = codeTemplate.Create<T>();
            codeTemplate.CopyPropertiesTo(classTemplate);

            //创建根目录
            rootDirectory = Path.GetFullPath(rootDirectory);
            if (!Directory.Exists(rootDirectory))
                Directory.CreateDirectory(rootDirectory);

            foreach (Entity entity in entityContext.Entities)
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
