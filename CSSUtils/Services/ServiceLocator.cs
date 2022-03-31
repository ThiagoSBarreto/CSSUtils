using System;
using System.Collections.Generic;
using System.Linq;

namespace CSSUtils.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private Dictionary<string, object> _singleTons;
        private Dictionary<string, object> _services;
        private static readonly ServiceLocator _instance = new ServiceLocator();
        public static ServiceLocator Current { get { return _instance; } }

        private ServiceLocator()
        {
            _singleTons = new Dictionary<string, object>();
            _services = new Dictionary<string, object>();
        }

        public void RegisterSingleton<T, T2>() where T2 : class
        {
            var check = typeof(T2).GetInterface(typeof(T).Name);
            if (check == null)
            {
                throw new ApplicationException($"The Singleton \"{typeof(T2).Name}\" must implement the interface \"{typeof(T).Name}\" to be registered.");
            }
            if (_singleTons.Keys.Contains(typeof(T).Name))
            {
                throw new ApplicationException($"Singleton allready Registered: \"{typeof(T).Name}\"");
            }
            _singleTons.Add(typeof(T).Name, null);
        }

        public T GetSingleton<T>()
        {
            try
            {
                if (_singleTons[typeof(T).Name] == null)
                {
                    _singleTons[typeof(T).Name] = Activator.CreateInstance(typeof(T));
                }
                return (T)_singleTons[typeof(T).Name];
            }
            catch
            {
                throw new ApplicationException($"Singleton Not Found \"{typeof(T).Name}\"");
            }
        }

        public void RegisterService<T>() where T : class
        {
            var instance = Activator.CreateInstance(typeof(T));
            string name = typeof(T).Name;
            if (_services.Keys.Contains(name))
            {
                throw new ApplicationException($"Service allready Registered: \"{name}\"");
            }
            _services.Add(name, instance);

            (instance as IServiceBase).Configure();
            (instance as IServiceBase).Start();
        }

        public T FindService<T>() where T : class
        {
            string name = typeof(T).Name;
            if (!_services.Keys.Contains(name))
            {
                throw new ApplicationException($"Service Not Found \"{name}\"");
            }
            return (T)_services[name];
        }

        public void UnRegisterService<T>() where T : class
        {
            string name = typeof(T).Name;
            if (_services.Keys.Contains(name))
            {
                (_services[name] as IServiceBase).Stop();
                (_services[name] as IServiceBase).Dispose();
                _services.Remove(name);
            }
        }

        public bool StartService<T>() where T : class
        {
            string name = typeof(T).Name;
            if (_services.Keys.Contains(name))
            {
                (_services[name] as IServiceBase).Start();
                return true;
            }
            return false;
        }

        public bool StopService<T>() where T : class
        {
            string name = typeof(T).Name;
            if (_services.Keys.Contains(name))
            {
                (_services[name] as IServiceBase).Stop();
                return true;
            }
            return false;
        }
    }
}
