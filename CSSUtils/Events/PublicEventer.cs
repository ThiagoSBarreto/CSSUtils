using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CSSUtils.Events
{
    public abstract class PublicEventer<T> : IPublicEventer<T>
    {
        private List<EventCallBack> _callBacks = new List<EventCallBack>();

        public void Subscribe(Action<T> payload, ThreadOptions options = ThreadOptions.MainThread)
        {
            _callBacks.Add(new EventCallBack(payload, options));
        }

        public void Unsubscribe(Action<T> payload)
        {
            _callBacks = _callBacks.Where(w => w.CallBack != payload).ToList();
        }

        public void Publish(T payload)
        {
            foreach(EventCallBack evcb in _callBacks)
            {
                switch (evcb.ThreadOptions)
                {
                    case ThreadOptions.MainThread:
                        evcb.CallBack.Invoke(payload);
                        break;
                    case ThreadOptions.BackgroundThread:
                        Task task = Task.Factory.StartNew(() =>
                        {
                            evcb.CallBack.Invoke(payload);
                        });
                        break;
                    case ThreadOptions.UIThread:
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            evcb.CallBack.Invoke(payload);
                        });
                        break;
                }
            }
        }

        private sealed class EventCallBack
        {
            public Action<T> CallBack { get; set; }
            public ThreadOptions ThreadOptions { get; set; }

            public EventCallBack(Action<T> ac, ThreadOptions options)
            {
                CallBack = ac;
                ThreadOptions = options;
            }
        }
    }

    public abstract class PublicEventer : IPublicEventer
    {
        private List<EventCallBack> _callBacks = new List<EventCallBack>();

        public void Subscribe(Action payload, ThreadOptions options = ThreadOptions.MainThread)
        {
            _callBacks.Add(new EventCallBack(payload, options));
        }

        public void Unsubscribe(Action payload)
        {
            _callBacks = _callBacks.Where(w => w.CallBack != payload).ToList();
        }

        public void Publish()
        {
            foreach (EventCallBack evcb in _callBacks)
            {
                switch (evcb.ThreadOptions)
                {
                    case ThreadOptions.MainThread:
                        evcb.CallBack.Invoke();
                        break;
                    case ThreadOptions.BackgroundThread:
                        Task task = new Task(() =>
                        {
                            evcb.CallBack.Invoke();
                        });
                        task.Start();
                        break;
                    case ThreadOptions.UIThread:
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            evcb.CallBack.Invoke();
                        });
                        break;
                }
            }
        }

        private sealed class EventCallBack
        {
            public Action CallBack { get; set; }
            public ThreadOptions ThreadOptions { get; set; }

            public EventCallBack(Action ac, ThreadOptions options)
            {
                CallBack = ac;
                ThreadOptions = options;
            }
        }
    }
}
