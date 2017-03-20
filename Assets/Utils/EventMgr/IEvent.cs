using System;
using System.Collections.Generic;

namespace EventDispatcherSpace
{
    /// <summary>
    /// IEvent接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// type类型.
        /// </value>
        string type { get; set; }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// target目标.
        /// </value>
        object target { get; set; }
    }
}
