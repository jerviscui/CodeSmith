using System;
using System.Collections.Generic;
using CodeSmith.Abp.Parser;

namespace CodeSmith.Abp.Extensions
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<string, string> _csharpTypeAlias;

        static TypeExtensions()
        {
            _csharpTypeAlias = new Dictionary<string, string>(16)
            {
                {"System.Int16", "short"},
                {"System.Int32", "int"},
                {"System.Int64", "long"},
                {"System.String", "string"},
                {"System.Object", "object"},
                {"System.Boolean", "bool"},
                {"System.Void", "void"},
                {"System.Char", "char"},
                {"System.Byte", "byte"},
                {"System.UInt16", "ushort"},
                {"System.UInt32", "uint"},
                {"System.UInt64", "ulong"},
                {"System.SByte", "sbyte"},
                {"System.Single", "float"},
                {"System.Double", "double"},
                {"System.Decimal", "decimal"}
            };
        }

        public static string ToType(this Type type, CodeLanguage language = CodeLanguage.CSharp)
        {
            return ToType(type.FullName, language);
        }

        public static string ToType(this string type, CodeLanguage language = CodeLanguage.CSharp)
        {
            if (type == "System.Xml.XmlDocument")
                type = "System.String";

            string t;
            if (language == CodeLanguage.CSharp && _csharpTypeAlias.TryGetValue(type, out t))
                return t;


            return type;
        }

        public static string ToNullableType(this Type type, bool isNullable = false, CodeLanguage language = CodeLanguage.CSharp)
        {
            return ToNullableType(type.FullName, isNullable, language);
        }

        public static string ToNullableType(this string type, bool isNullable = false, CodeLanguage language = CodeLanguage.CSharp)
        {
            bool isValueType = type.IsValueType();

            type = type.ToType(language);

            if (!isValueType || !isNullable)
                return type;

            return language == CodeLanguage.VisualBasic
              ? string.Format("Nullable(Of {0})", type)
              : type + "?";
        }

        public static bool IsValueType(this string type)
        {
            if (!type.StartsWith("System."))
                return false;

            var t = Type.GetType(type, false);
            return t != null && t.IsValueType;
        }
    }
}
