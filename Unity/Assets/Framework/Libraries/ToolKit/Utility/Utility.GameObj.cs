using UnityEngine;

namespace Framework
{
    public static partial class Utility
    {
        public static partial class GameObj
        {
            /// <summary>
            /// 查找GameObject,已经拆分好的路径
            /// </summary>
            /// <param name="parent">父物体，无的话就是取自根结构</param>
            /// <param name="subPath">拆分后的路径</param>
            /// <param name="index">拆分后的路径索引</param>
            /// <param name="build">是否需要创建GameObject</param>
            /// <param name="dontDestroy">是否不要销毁</param>
            /// <returns>指定的GameObject</returns>
            public static GameObject FindGameObject(GameObject parent, string[] subPath, int index, bool build,
                bool dontDestroy)
            {
                GameObject obj = null;
                if (parent == null)
                {
                    obj = GameObject.Find(subPath[index]);
                }
                else
                {
                    var objTrans = parent.transform.Find(subPath[index]);
                    if (objTrans != null)
                    {
                        obj = objTrans.gameObject;
                    }
                }

                if (obj == null)
                {
                    if (build)
                    {
                        obj = new GameObject(subPath[index]);
                        if (parent != null)
                        {
                            obj.transform.SetParent(parent.transform);
                        }

                        if (dontDestroy && index == 0)
                        {
                            Object.DontDestroyOnLoad(obj);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                return ++index == subPath.Length ? obj : FindGameObject(parent, subPath, index, build, dontDestroy);
            }

            /// <summary>
            /// 查找GameObject,依据路径
            /// </summary>
            /// <param name="path">路径</param>
            /// <param name="build">是否需要创建GameObject</param>
            /// <param name="dontDestroy">是否不要销毁</param>
            /// <returns>指定的GameObject</returns>
            public static GameObject FindGameObject(string path, bool build, bool dontDestroy)
            {
                if (string.IsNullOrEmpty(path))
                {
                    Log.Error("Find GameObject ： path is null!!!");
                    return null;
                }

                var subPath = path.Contains("/") ? path.Split('/') : new[] { path };
                return FindGameObject(null, subPath, 0, build, dontDestroy);
            }

            /// <summary>
            /// 查找GameObject 并在其上增加T组件
            /// </summary>
            /// <param name="path">GameObject路径</param>
            /// <param name="dontDestroy">是否不要销毁</param>
            /// <typeparam name="T"></typeparam>
            /// <returns>T组件</returns>
            public static T CreateComponentOnGameObject<T>(string path, bool dontDestroy) where T : class
            {
                var obj = FindGameObject(path, true, dontDestroy);
                if (obj == null)
                {
                    obj = new GameObject("Singleton of " + typeof(T).Name);
                    if (dontDestroy)
                    {
                        Object.DontDestroyOnLoad(obj);
                    }
                }

                return obj.AddComponent(typeof(T)) as T;
            }
        }
    }
}