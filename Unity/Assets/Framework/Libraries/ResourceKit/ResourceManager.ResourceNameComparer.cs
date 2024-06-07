// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 11:29:1
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源名称比较器
        /// </summary>
        private sealed class ResourceNameComparer : IComparer<ResourceName>, IEqualityComparer<ResourceName>
        {
            public int Compare(ResourceName x, ResourceName y)
            {
                return x.CompareTo(y);
            }

            public bool Equals(ResourceName x, ResourceName y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(ResourceName obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}