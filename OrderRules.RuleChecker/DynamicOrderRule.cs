using OrderRules.Interface;

namespace OrderRules.RuleChecker
{
    public class DynamicOrderRule
    {
        public IOrderRule OrderRule { get; private set; }
        public string TypeName { get; private set; }
        public string AssemblyName { get; private set; }
        public string Message { get; set; }

        public DynamicOrderRule(IOrderRule orderRule, string typeName, string assemblyName)
        {
            OrderRule = orderRule;
            TypeName = typeName;
            AssemblyName = assemblyName;
        }
    }
}
