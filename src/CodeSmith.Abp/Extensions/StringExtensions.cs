using System;
using System.Collections.Generic;
using CodeSmith.Abp.Parser;
using CodeSmith.Engine;
using CodeSmith.Engine.Licensing;

namespace CodeSmith.Abp.Extensions
{
    public static class StringExtensions
    {
        private static readonly HashSet<string> _csharpKeywords;
        private static readonly HashSet<string> _visualBasicKeywords;

        static StringExtensions()
        {
            _csharpKeywords = new HashSet<string>(StringComparer.Ordinal)
            {
                "as", "do", "if", "in", "is",
                "for", "int", "new", "out", "ref", "try",
                "base", "bool", "byte", "case", "char", "else", "enum", "goto", "lock", "long", "null", "this", "true", "uint", "void",
                "break", "catch", "class", "const", "event", "false", "fixed", "float", "sbyte", "short", "throw", "ulong", "using", "while",
                "double", "extern", "object", "params", "public", "return", "sealed", "sizeof", "static", "string", "struct", "switch", "typeof", "unsafe", "ushort",
                "checked", "decimal", "default", "finally", "foreach", "private", "virtual",
                "abstract", "continue", "delegate", "explicit", "implicit", "internal", "operator", "override", "readonly", "volatile",
                "__arglist", "__makeref", "__reftype", "interface", "namespace", "protected", "unchecked",
                "__refvalue", "stackalloc"
            };

            _visualBasicKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "as", "do", "if", "in", "is", "me", "of", "on", "or", "to",
                "and", "dim", "end", "for", "get", "let", "lib", "mod", "new", "not", "rem", "set", "sub", "try", "xor",
                "ansi", "auto", "byte", "call", "case", "cdbl", "cdec", "char", "cint", "clng", "cobj", "csng", "cstr", "date", "each", "else", 
                "enum", "exit", "goto", "like", "long", "loop", "next", "step", "stop", "then", "true", "wend", "when", "with",
                "alias", "byref", "byval", "catch", "cbool", "cbyte", "cchar", "cdate", "class", "const", "ctype", "cuint", "culng", "endif", "erase", "error", 
                "event", "false", "gosub", "isnot", "redim", "sbyte", "short", "throw", "ulong", "until", "using", "while",
                "csbyte", "cshort", "double", "elseif", "friend", "global", "module", "mybase", "object", "option", "orelse", "public", "resume", "return", "select", "shared", 
                "single", "static", "string", "typeof", "ushort",
                "andalso", "boolean", "cushort", "decimal", "declare", "default", "finally", "gettype", "handles", "imports", "integer", "myclass", "nothing", "partial", "private", "shadows", 
                "trycast", "unicode", "variant",
                "assembly", "continue", "delegate", "function", "inherits", "operator", "optional", "preserve", "property", "readonly", "synclock", "uinteger", "widening",
                "addressof", "interface", "namespace", "narrowing", "overloads", "overrides", "protected", "structure", "writeonly",
                "addhandler", "directcast", "implements", "paramarray", "raiseevent", "withevents",
                "mustinherit", "overridable",
                "mustoverride",
                "removehandler",
                "class_finalize", "notinheritable", "notoverridable",
                "class_initialize"
            };
        }

        public static string ToCamelCase(this string name)
        {
            return StringUtil.ToCamelCase(name);
        }

        public static string ToPluralCamelCase(this string name)
        {
            return StringUtil.ToCamelCase(StringUtil.ToPlural(name));
        }

        public static string ToSingularCamelCase(this string name)
        {
            return StringUtil.ToCamelCase(StringUtil.ToSingular(name));
        }

        public static string ToPascalCase(this string name)
        {
            return StringUtil.ToPascalCase(name);
        }

        public static string ToPluralPascalCase(this string name)
        {
            return StringUtil.ToPascalCase(StringUtil.ToPlural(name));
        }

        public static string ToSingularPascalCase(this string name)
        {
            return StringUtil.ToPascalCase(StringUtil.ToSingular(name));
        }

        public static string ToFormat(this string value, params object[] values)
        {
            return string.Format(value, values);
        }

        public static string ToFieldName(this string name)
        {
            return "_" + StringUtil.ToCamelCase(name);
        }

        public static string MakeUnique(this string name, Func<string, bool> exists)
        {
            string uniqueName = name;
            int count = 1;

            while (exists(uniqueName))
                uniqueName = string.Concat(name, count++);

            return uniqueName;
        }

        public static bool IsKeyword(this string text, CodeLanguage language = CodeLanguage.CSharp)
        {
            return language == CodeLanguage.VisualBasic
              ? _visualBasicKeywords.Contains(text)
              : _csharpKeywords.Contains(text);
        }

        public static string ToSafeName(this string name, CodeLanguage language = CodeLanguage.CSharp)
        {
            if (!name.IsKeyword(language))
                return name;

            return language == CodeLanguage.VisualBasic
              ? string.Format("[{0}]", name)
              : "@" + name;
        }

        public static bool IsNull(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        //todo 将占位符替换成注释

        public static string ToReplaceLin(this string value)
        {
            return value.Replace("\n", " ").Replace("\r", " ");
        }
    }
}
