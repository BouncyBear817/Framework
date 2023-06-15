using System;

namespace Framework
{
    /// <summary>
    /// MonoSingleton 生成路径，只应用于class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonPath : Attribute
    {
        public string PathInHierarchy { get; }
        
        public MonoSingletonPath(string pathInHierarchy)
        {
            PathInHierarchy = pathInHierarchy;
        }
    }
}