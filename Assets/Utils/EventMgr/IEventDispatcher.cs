using System;
using System.Collections.Generic;

namespace EventDispatcherSpace
{
    /// <summary>
    /// IEventDispatcher 事件分发接口，添加移除分发
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <returns><c>true</c>, 如果事件成功被监听， <c>false</c> 失败 </returns>
        /// <param name="aEventType_string">A event type_string.</param>
        /// <param name="aEventDelegate">一个事件委托</param>
        bool addEventListener(string aEventType_string, EventDelegate aEventDelegate);

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <returns><c>true</c>, 如果事件成功被监听, <c>false</c> 失败 </returns>
        /// <param name="aEventType_string">A event type_string.</param>
        /// <param name="aEventDelegate">一个事件委托</param>
        /// <param name="eventDispatcherAddMode">Event dispatcher add mode.</param>
        bool addEventListener(string aEventType_string, EventDelegate aEventDelegate, EventDispatcherAddMode eventDispatcherAddMode);


        /// <summary>
        /// 是否有这个事件监听
        /// </summary>
        /// <returns><c>true</c>, 这个监听已经有了, <c>false</c> 没有.</returns>
        /// <param name="aEventType_string">A event type_string.</param>
        /// <param name="aEventDelegate">一个事件委托</param>
        bool hasEventListener(string aEventType_string, EventDelegate aEventDelegate);

        /// <summary>
        /// 移除事件监听
        /// </summary>
        /// <returns><c>true</c>, 被成功移除 <c>false</c> 失败</returns>
        /// <param name="aEventType_string">A event type_string.</param>
        /// <param name="aEventDelegate">一个事件委托</param>
        bool removeEventListener(string aEventType_string, EventDelegate aEventDelegate);

        /// <summary>
        /// 移除所有事件监听
        /// </summary>
        /// <returns><c>true</c>, 成功, <c>false</c> 失败 </returns>
        bool removeAllEventListeners();

        /// <summary>
        /// 分发广播事件
        /// </summary>
        /// <returns><c>true</c>, 成功, <c>false</c> 失败</returns>
        /// <param name="aIEvent">A I event.</param>
        bool dispatchEvent(IEvent aIEvent);
    }
}
