using System;
using System.Collections.Generic;
using System.Linq;

namespace CSSUtils.Events
{
    public class Eventer : IEventer
    {
        private Dictionary<object, object> _events = new Dictionary<object, object>();

        /// <summary>
        /// Gets the event by its Class name, if not registered, it will be added
        /// </summary>
        /// <typeparam name="T">The Class name of the Event</typeparam>
        /// <returns></returns>
        public T GetEvent<T>()
        {
            if (_events.Keys.Contains(typeof(T)))
            {
                return (T)_events[typeof(T)];
            }
            else
            {
                _events.Add(typeof(T), Activator.CreateInstance(typeof(T)));
                return (T)_events[typeof(T)];
            }
        }
    }
}
