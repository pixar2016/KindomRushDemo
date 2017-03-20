using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventDispatcherSpace
{
    public class SampleObservedComponent : MonoBehaviour
    {
        /// <summary>
        /// The event dispatcher.
        /// </summary>
        public EventDispatcher eventDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="com.rmc.projects.event_dispatcher.SampleObservedComponent"/> class.
        /// </summary>
        public SampleObservedComponent()
        {

            eventDispatcher = new EventDispatcher(this);
        }

        ///<summary>
        ///    Use this for initialization
        ///</summary>
        public void Start()
        {
            SampleEvent sampleEvent = new SampleEvent(SampleEvent.SAMPLE_EVENT);
            sampleEvent.customValue = "foo";
            Debug.Log("Dispatching: SampleEvent " + sampleEvent);
            eventDispatcher.dispatchEvent(sampleEvent);
        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        public void OnDestroy()
        {
            //    CLEANUP MEMORY
            eventDispatcher.removeAllEventListeners();
            eventDispatcher = null;
        }
    }
}
