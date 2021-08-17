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
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="options"></param>
        void Subscribe(Action<T> payload, ThreadOptions options = ThreadOptions.MainThread);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        void Unsubscribe(Action<T> payload);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        void Publish(T payload);
    }

    public interface IPublicEventer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="options"></param>
        void Subscribe(Action payload, ThreadOptions options = ThreadOptions.MainThread);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param>
        void Unsubscribe(Action payload);

        /// <summary>
        /// 
        /// </summary>
        void Publish();
    }
}
