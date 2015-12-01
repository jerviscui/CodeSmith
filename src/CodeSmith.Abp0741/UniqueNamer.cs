﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CodeSmith.Abp0741
{
    /// <summary>
    /// 名称唯一处理类
    /// </summary>
    public class UniqueNamer
    {
        private readonly ConcurrentDictionary<string, HashSet<string>> _names;

        public UniqueNamer()
        {
            _names = new ConcurrentDictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
            Comparer = StringComparer.CurrentCulture;

            // add existing
            UniqueContextName("ChangeTracker");
            UniqueContextName("Configuration");
            UniqueContextName("Database");
            UniqueContextName("InternalContext");
        }

        public IEqualityComparer<string> Comparer { get; set; }

        public string UniqueName(string bucketName, string name)
        {
            var hashSet = _names.GetOrAdd(bucketName, k => new HashSet<string>(Comparer));
            string result = name.MakeUnique(hashSet.Contains);
            hashSet.Add(result);

            return result;
        }

        public string UniqueClassName(string className)
        {
            const string globalClassName = "global::ClassName";
            return UniqueName(globalClassName, className);
        }

        public string UniqueContextName(string name)
        {
            const string globalContextname = "global::ContextName";
            return UniqueName(globalContextname, name);
        }

        public string UniqueRelationshipName(string name)
        {
            const string globalContextname = "global::RelationshipName";
            return UniqueName(globalContextname, name);
        }
    }
}