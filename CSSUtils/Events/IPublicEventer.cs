using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.Events
{
    public interface IPublicEventer<T>
    {
        /// <summary>
        /// Subscribe a Method to the Event, whenever it's raised, this Method will be executed in the configured Thread.
        /// This Subscribe requires a TYPE to the payload
        /// </summary>
        /// <param name="payload">Callback Method</param>
        /// <param name="options">The type of Thread that will be used to execute the Callback Method</param>
        void Subscribe(Action<T> payload, ThreadOptions options = ThreadOptions.MainThread);

        /// <summary>
        /// Unsubscribe the Method to the Event, the Callback Method must be passed to Unsubscribe
        /// </summary>
        /// <param name="payload">Callback Method used in the Subscription</param>
        void Unsubscribe(Action<T> payload);

        /// <summary>
        /// Publish the payload to the event and call all subscribed Callback Methods
        /// </summary>
        /// <param name="payload"></param>
        void Publish(T payload);
    }

    public interface IPublicEventer
    {
        /// <summary>
        /// Subscribe a Method to the Event, whenever it's raised, this Method will be executed in the configured Thread
        /// This Subscribe DOESN'T use payloads
        /// </summary>
        /// <param name="payload">Callback Method</param>
        /// <param name="options">The type of Thread that will be used to execute the Callback Method</param>
        void Subscribe(Action payload, ThreadOptions options = ThreadOptions.MainThread);

        /// <summary>
        /// Unsubscribe the Method to the Event, the Callback Method must be passed to Unsubscribe
        /// </summary>
        /// <param name="payload">Callback Method used in the Subscription</param>
        void Unsubscribe(Action payload);

        /// <summary>
        /// Publish the payload to the event and call all subscribed Callback Methods
        /// </summary>
        void Publish();
    }
}
