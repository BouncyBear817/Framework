// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/21 11:29:1
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;
using System.Collections.Generic;

namespace Framework
{
    public sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        private struct ResourceName : IComparable, IComparable<ResourceName>, IEquatable<ResourceName>
        {
            private static readonly Dictionary<ResourceName, string> sResourceFullNames =
                new Dictionary<ResourceName, string>();

            private readonly string mName;
            private readonly string mVariant;
            private readonly string mExtension;

            public ResourceName(string name, string variant, string extension)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Resource name is invalid.");
                }

                if (string.IsNullOrEmpty(extension))
                {
                    throw new Exception("Resource extension is invalid.");
                }

                mName = name;
                mVariant = variant;
                mExtension = extension;
            }

            /// <summary>
            /// 资源名称
            /// </summary>
            public string Name => mName;

            /// <summary>
            /// 变体名称
            /// </summary>
            public string Variant => mVariant;

            /// <summary>
            /// 扩展名称
            /// </summary>
            public string Extension => mExtension;

            /// <summary>
            /// 资源完整名称
            /// </summary>
            public string FullName
            {
                get
                {
                    if (sResourceFullNames.TryGetValue(this, out var fullName))
                    {
                        return fullName;
                    }

                    fullName = mVariant != null ? $"{mName}.{mVariant}.{mExtension}" : $"{mName}.{mExtension}";
                    sResourceFullNames.Add(this, fullName);
                    return fullName;
                }
            }

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return 1;
                }

                if (!(obj is ResourceName))
                {
                    throw new Exception("Type of obj is invalid.");
                }

                return CompareTo((ResourceName)obj);
            }

            public int CompareTo(ResourceName other)
            {
                var result = string.CompareOrdinal(mName, other.mName);
                if (result != 0)
                {
                    return result;
                }

                result = string.CompareOrdinal(mVariant, other.mVariant);
                if (result != 0)
                {
                    return result;
                }

                return result = string.CompareOrdinal(mExtension, other.mExtension);
            }

            public bool Equals(ResourceName other)
            {
                return string.Equals(mName, other.mName, StringComparison.Ordinal) &&
                       string.Equals(mVariant, other.mVariant, StringComparison.Ordinal) &&
                       string.Equals(mExtension, other.mExtension, StringComparison.Ordinal);
            }

            public override bool Equals(object obj)
            {
                return (obj is ResourceName) && Equals((ResourceName)obj);
            }

            public override int GetHashCode()
            {
                if (mVariant == null)
                {
                    return mName.GetHashCode() ^ mExtension.GetHashCode();
                }

                return mName.GetHashCode() ^ mVariant.GetHashCode() ^ mExtension.GetHashCode();
            }

            public override string ToString()
            {
                return FullName;
            }

            public static bool operator ==(ResourceName a, ResourceName b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(ResourceName a, ResourceName b)
            {
                return !(a == b);
            }
        }
    }
}