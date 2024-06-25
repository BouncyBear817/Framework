// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/6/25 11:33:27
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Framework.Editor
{
    public static class HelperType
    {
        private static readonly string[] RuntimeAssemblyNames =
        {
            "Framework.Runtime",
            "Assembly-CSharp"
        };

        private static readonly string[] RuntimeOrEditorAssemblyNames =
        {
            "Framework.Runtime",
            "Assembly-CSharp",
            "Framework.Editor",
            "Assembly-CSharp-Editor"
        };

        public static string GetConfigurationPath<T>() where T : ConfigPathAttribute
        {
            foreach (var type in Utility.Assembly.GetTypes())
            {
                if (!type.IsAbstract || !type.IsSealed)
                {
                    continue;
                }

                foreach (var fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (fieldInfo.FieldType == typeof(string) && fieldInfo.IsDefined(typeof(T), false))
                    {
                        return (string)fieldInfo.GetValue(null);
                    }
                }

                foreach (var propertyInfo in type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (propertyInfo.PropertyType == typeof(string) && propertyInfo.IsDefined(typeof(T), false))
                    {
                        return (string)propertyInfo.GetValue(null);
                    }
                }
            }

            return null;
        }

        public static string[] GetRuntimeTypeNames(Type type)
        {
            return GetTypeNames(type, RuntimeAssemblyNames);
        }

        public static string[] GetRuntimeOrEditorTypeNames(Type type)
        {
            return GetTypeNames(type, RuntimeOrEditorAssemblyNames);
        }

        private static string[] GetTypeNames(Type typeBase, string[] assemblyNames)
        {
            var typeNames = new List<string>();
            foreach (var assemblyName in assemblyNames)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                {
                    continue;
                }

                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }
    }
}