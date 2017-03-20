using System;
using System.Collections.Generic;


namespace EventDispatcherSpace
{
    /// <summary>
    /// 事件事件接口
    /// </summary>
    public class Event : IEvent
    {
        // GETTER / SETTER
        /// <summary>
        /// The _type_string.
        /// </summary>
        private string _type_string;
        string IEvent.type
        {
            get
            {
                return _type_string;
            }
            set
            {
                _type_string = value;

            }
        }

        /// <summary>
        /// The _target_object.
        /// </summary>
        private object _target_object;
        object IEvent.target
        {
            get
            {
                return _target_object;
            }
            set
            {
                _target_object = value;

            }
        }

        ///<summary>
        ///     Constructor
        ///</summary>
        public Event(string aType_str)
        {
            //
            _type_string = aType_str;
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        //~Event ( )
        //{
        //    Debug.Log ("Event.deconstructor()");
        //}
    }
}
