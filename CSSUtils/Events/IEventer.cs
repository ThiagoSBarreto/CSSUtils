using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.Events
{
    public interface IEventer
    {
        /// <summary>
        /// Gets the event from the event list, if not existent, the event will be added
        /// </summary>
        /// <typeparam name="T">Event Class</typeparam>
        /// <returns></returns>
        T GetEvent<T>();
    }

    public enum ThreadOptions
    {
        /// <summary>
        /// Executes the Callback Method in the same Thread as the caller.
        /// </summary>
        MainThread,
        /// <summary>
        /// Pass the execution of the Callback Method to the UI Thread
        /// </summary>
        UIThread,
        /// <summary>
        /// Executes the Callback Method in a new Background Thread
        /// </summary>
        BackgroundThread,
    }
}
