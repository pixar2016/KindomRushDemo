using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventDispatcherSpace
{
    public class SampleObserverComponent : MonoBehaviour
    {
        /// <summary>
        /// The sample observed game object.
        /// </summary>
        public SampleObservedComponent sampleObservedGameObject;

        public void Start()
        {
            /*
            * 注意： 这里observer和observed是 MonoBehavior 子类
            **/
            sampleObservedGameObject.eventDispatcher.addEventListener(SampleEvent.SAMPLE_EVENT, _onSampleEvent);

        }

        void Update()
        {
            //Debug.Log (sampleObservedGameObject.eventDispatcher);
        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        public void OnDestroy()
        {
            //    CLEANUP MEMORY
            sampleObservedGameObject.eventDispatcher.removeEventListener(SampleEvent.SAMPLE_EVENT, _onSampleEvent);

        }
        //--------------------------------------
        //  Events
        //--------------------------------------
        /// <summary>
        /// _ons the sample event.
        /// </summary>
        /// <param name="aIEvent">A I event.</param>
        public void _onSampleEvent(IEvent aIEvent)
        {
            Debug.Log("\tListening: _onSampleEvent() aIEvent: " + aIEvent + " with customValue: " + (aIEvent as SampleEvent).customValue);
        }
    }
}
