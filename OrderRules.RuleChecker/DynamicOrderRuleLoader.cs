using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using OrderRules.Interface;

namespace OrderRules.RuleChecker
{
    public class DynamicOrderRuleLoader
    {
        public static List<DynamicOrderRule> LoadRules(string assemblyPath)
        {
            var rules = new List<DynamicOrderRule>();

            if (!Directory.Exists(assemblyPath))
                return rules;

            IEnumerable<string> assemblyFiles = Directory.EnumerateFiles(assemblyPath, "*.dll", SearchOption.TopDirectoryOnly);

            foreach (string assemblyFile in assemblyFiles)
            {
                Assembly assembly = Assembly.LoadFrom(assemblyFile);
                foreach (Type type in assembly.ExportedTypes)
                {
                    if (type.IsClass && typeof(IOrderRule).IsAssignableFrom(type))
                    {
                        IOrderRule rule = Activator.CreateInstance(type) as IOrderRule;
                        rules.Add(new DynamicOrderRule(rule, type.FullName, type.Assembly.GetName().Name));
                    }
                }
            }

            return rules;
        }
    }
}
