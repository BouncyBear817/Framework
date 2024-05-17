// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/5/16 11:30:18
//  * Description:
//  * Modify Record:
//  *************************************************************/

using System;

namespace Framework
{
    public static partial class Utility
    {
        /// <summary>
        /// Marshal相关的实用函数
        /// </summary>
        public static class Marshal
        {
            private const int BlockSize = 1024 * 4;
            private static IntPtr sCachedHGlobalIntPtr = IntPtr.Zero;
            private static int sCachedHGlobalSize = 0;

            /// <summary>
            /// 缓存的从进程的非托管内存中分配的内存的大小
            /// </summary>
            public static int CachedHGlobalSize => sCachedHGlobalSize;

            /// <summary>
            /// 确保从进程的非托管内存中分配足够大小的内存并缓存
            /// </summary>
            /// <param name="ensureSize">确保分配的内存大小</param>
            /// <exception cref="Exception"></exception>
            public static void EnsureCachedHGlobalSize(int ensureSize)
            {
                if (ensureSize < 0)
                {
                    throw new Exception("Ensure size is invalid.");
                }

                if (sCachedHGlobalIntPtr == IntPtr.Zero || sCachedHGlobalSize < ensureSize)
                {
                    FreeCachedHGlobal();
                    var size = (ensureSize - 1 + BlockSize) / BlockSize * BlockSize;
                    sCachedHGlobalIntPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
                    sCachedHGlobalSize = size;
                }
            }

            /// <summary>
            /// 释放从进程的非托管内存中分配的内存
            /// </summary>
            public static void FreeCachedHGlobal()
            {
                if (sCachedHGlobalIntPtr != IntPtr.Zero)
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(sCachedHGlobalIntPtr);
                    sCachedHGlobalIntPtr = IntPtr.Zero;
                    sCachedHGlobalSize = 0;
                }
            }

            /// <summary>
            /// 将对象转换为二进制流
            /// </summary>
            /// <param name="structure">对象</param>
            /// <typeparam name="T">对象的类型</typeparam>
            /// <returns>转换后的二进制流</returns>
            public static byte[] StructureToBytes<T>(T structure)
            {
                return StructureToBytes(structure, System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
            }

            /// <summary>
            /// 将对象转换为二进制流
            /// </summary>
            /// <param name="structure">对象</param>
            /// <param name="structureSize">对象的大小</param>
            /// <typeparam name="T">对象的类型</typeparam>
            /// <returns>转换后的二进制流</returns>
            /// <exception cref="Exception"></exception>
            public static byte[] StructureToBytes<T>(T structure, int structureSize)
            {
                if (structureSize < 0)
                {
                    throw new Exception("Structure size is invalid.");
                }

                EnsureCachedHGlobalSize(structureSize);
                System.Runtime.InteropServices.Marshal.StructureToPtr(structure, sCachedHGlobalIntPtr, true);
                var result = new byte[structureSize];
                System.Runtime.InteropServices.Marshal.Copy(sCachedHGlobalIntPtr, result, 0, structureSize);
                return result;
            }

            /// <summary>
            /// 将对象转换为二进制流
            /// </summary>
            /// <param name="structure">对象</param>
            /// <param name="result">转换后的二进制流</param>
            /// <typeparam name="T">对象的类型</typeparam>
            public static void StructureToBytes<T>(T structure, byte[] result)
            {
                StructureToBytes(structure, System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), result,
                    0);
            }

            /// <summary>
            /// 将对象转换为二进制流
            /// </summary>
            /// <param name="structure">对象</param>
            /// <param name="structureSize">对象的大小</param>
            /// <param name="result">转换后的二进制流</param>
            /// <typeparam name="T">对象的类型</typeparam>
            public static void StructureToBytes<T>(T structure, int structureSize, byte[] result)
            {
                StructureToBytes(structure, structureSize, result, 0);
            }

            /// <summary>
            /// 将对象转换为二进制流
            /// </summary>
            /// <param name="structure">对象</param>
            /// <param name="result">转换后的二进制流</param>
            /// <param name="startIndex">转换二进制流的起始位置</param>
            /// <typeparam name="T">对象的类型</typeparam>
            public static void StructureToBytes<T>(T structure, byte[] result, int startIndex)
            {
                StructureToBytes(structure, System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), result,
                    startIndex);
            }

            /// <summary>
            /// 将对象转换为二进制流
            /// </summary>
            /// <param name="structure">对象</param>
            /// <param name="structureSize">对象的大小</param>
            /// <param name="result">转换后的二进制流</param>
            /// <param name="startIndex">转换二进制流的起始位置</param>
            /// <typeparam name="T">对象的类型</typeparam>
            /// <exception cref="Exception"></exception>
            public static void StructureToBytes<T>(T structure, int structureSize, byte[] result, int startIndex)
            {
                if (structureSize < 0)
                {
                    throw new Exception("Structure size is invalid.");
                }

                if (result == null)
                {
                    throw new Exception("Result is invalid.");
                }

                if (startIndex < 0)
                {
                    throw new Exception("Start index is invalid.");
                }

                if (startIndex + structureSize > result.Length)
                {
                    throw new Exception("Result length is not enough.");
                }

                EnsureCachedHGlobalSize(structureSize);
                System.Runtime.InteropServices.Marshal.StructureToPtr(structure, sCachedHGlobalIntPtr, true);
                System.Runtime.InteropServices.Marshal.Copy(sCachedHGlobalIntPtr, result, startIndex, structureSize);
            }

            /// <summary>
            /// 将二进制流转换为对象
            /// </summary>
            /// <param name="buffer">二进制流</param>
            /// <typeparam name="T">对象类型</typeparam>
            /// <returns>转换后的对象</returns>
            public static T BytesToStructure<T>(byte[] buffer)
            {
                return BytesToStructure<T>(System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), buffer, 0);
            }

            /// <summary>
            /// 将二进制流转换为对象
            /// </summary>
            /// <param name="buffer">二进制流</param>
            /// <param name="startIndex">转换二进制流的起始位置</param>
            /// <typeparam name="T">对象的类型</typeparam>
            /// <returns>转换后的对象</returns>
            public static T BytesToStructure<T>(byte[] buffer, int startIndex)
            {
                return BytesToStructure<T>(System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), buffer,
                    startIndex);
            }

            /// <summary>
            /// 将二进制流转换为对象
            /// </summary>
            /// <param name="structureSize">对象的大小</param>
            /// <param name="buffer">二进制流</param>
            /// <typeparam name="T">对象的类型</typeparam>
            /// <returns>转换后的对象</returns>
            public static T BytesToStructure<T>(int structureSize, byte[] buffer)
            {
                return BytesToStructure<T>(structureSize, buffer, 0);
            }

            /// <summary>
            /// 将二进制流转换为对象
            /// </summary>
            /// <param name="structureSize">对象的大小</param>
            /// <param name="buffer">二进制流</param>
            /// <param name="startIndex">转换二进制流的起始位置</param>
            /// <typeparam name="T">对象的类型</typeparam>
            /// <returns>转换后的对象</returns>
            /// <exception cref="Exception"></exception>
            public static T BytesToStructure<T>(int structureSize, byte[] buffer, int startIndex)
            {
                if (structureSize < 0)
                {
                    throw new Exception("Structure size is invalid.");
                }

                if (buffer == null)
                {
                    throw new Exception("Buffer is invalid.");
                }

                if (startIndex < 0)
                {
                    throw new Exception("Start index is invalid.");
                }

                if (startIndex + structureSize > buffer.Length)
                {
                    throw new Exception("Result length is not enough.");
                }

                EnsureCachedHGlobalSize(structureSize);
                System.Runtime.InteropServices.Marshal.Copy(buffer, startIndex, sCachedHGlobalIntPtr, structureSize);
                return System.Runtime.InteropServices.Marshal.PtrToStructure<T>(sCachedHGlobalIntPtr);
            }
        }
    }
}