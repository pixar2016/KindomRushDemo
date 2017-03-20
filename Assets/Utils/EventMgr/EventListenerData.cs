using System;
using System.Collections.Generic;


namespace EventDispatcherSpace
{
    /// <summary>
    /// EventListenerData
    /// </summary>
    public class EventListenerData
    {
        /// <summary>
        /// The _event listener.
        /// </summary>
        private object _eventListener;
        public object eventListener
        {
            get
            {
                return _eventListener;
            }
            set
            {
                _eventListener = value;

            }
        }

        /// <summary>
        /// 事件名
        /// </summary>
        private string _eventName_string;
        public string eventName
        {
            get
            {
                return _eventName_string;
            }
            set
            {
                _eventName_string = value;

            }
        }



        /// <summary>
        /// 事件委托
        /// </summary>
        private EventDelegate _eventDelegate;
        public EventDelegate eventDelegate
        {
            get
            {
                return _eventDelegate;
            }
            set
            {
                _eventDelegate = value;

            }
        }

        private EventDispatcherAddMode _eventListeningMode;
        public EventDispatcherAddMode eventListeningMode
        {
            get
            {
                return _eventListeningMode;
            }
            set
            {
                _eventListeningMode = value;

            }
        }

        ///<summary>
        ///     Constructor
        ///</summary>
        public EventListenerData(object aEventListener, string aEventName_string, EventDelegate aEventDelegate, EventDispatcherAddMode aEventListeningMode)
        {
            _eventListener = aEventListener;
            _eventName_string = aEventName_string;
            _eventDelegate = aEventDelegate;
            _eventListeningMode = aEventListeningMode;
        }

        /// <summary>
        /// Deconstructor
        /// </summary>
        //~EventListenerData ( )
        //{
        //Debug.Log ("EventListenerData.deconstructor()");

        //}
    }
}
