/************************************************************
 * Unity Version: 2022.3.15f1c1
 * Author:        bear
 * CreateTime:    2024/01/30 16:12:04
 * Description:   
 * Modify Record: 
 *************************************************************/

using System;
using Framework;
using UnityEngine;

namespace Runtime
{
    /// <summary>
    /// 事件组件
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Framework/Event")]
    public sealed class EventComponent : FrameworkComponent
    {
        private IEventManager mEventManager = null;

        /// <summary>
        /// 事件处理函数的数量
        /// </summary>
        public int EventHandlerCount => mEventManager.EventHandlerCount;

        /// <summary>
        /// 事件数量
        /// </summary>
        public int EventCount => mEventManager.EventCount;

        protected override void Awake()
        {
            base.Awake();

            mEventManager = FrameworkEntry.GetModule<IEventManager>();
            if (mEventManager == null)
            {
                Log.Fatal("Event manager is invalid.");
            }
        }

        /// <summary>
        /// 事件处理函数的数量，指定id
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <returns>事件处理函数的数量</returns>
        public int Count(int id)
        {
            return mEventManager.Count(id);
        }

        /// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        /// <returns>是否存在事件处理函数</returns>
        public bool Check(int id, EventHandler<BaseEventArgs> handler)
        {
            return mEventManager.Check(id, handler);
        }

        /// <summary>
        /// 订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        public void Subscribe(int id, EventHandler<BaseEventArgs> handler)
        {
            mEventManager.Subscribe(id, handler);
        }

        /// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id">事件编号</param>
        /// <param name="handler">事件处理函数</param>
        public void UnSubscribe(int id, EventHandler<BaseEventArgs> handler)
        {
            mEventManager.UnSubscribe(id, handler);
        }

        /// <summary>
        /// 设置默认事件处理函数
        /// </summary>
        /// <param name="handler">事件处理函数</param>
        public void SetDefaultHandler(EventHandler<BaseEventArgs> handler)
        {
            mEventManager.SetDefaultHandler(handler);
        }

        /// <summary>
        /// 抛出事件，线程安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender, BaseEventArgs e)
        {
            mEventManager.Fire(sender, e);
        }

        /// <summary>
        /// 抛出事件，线程不安全
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender, BaseEventArgs e)
        {
            mEventManager.FireNow(sender, e);
        }
    }
}