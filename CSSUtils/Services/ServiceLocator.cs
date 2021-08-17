using System;
using System.Collections.Generic;
using System.Linq;

namespace CSSUtils.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceLocator : IServiceLocator
    {
        #region [ Atributos ]
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, object> _singleTons;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, object> _services;

        /// <summary>
        /// 
        /// </summary>
        private static readonly ServiceLocator _instance = new ServiceLocator();

        /// <summary>
        /// 
        /// </summary>
        public static ServiceLocator Current { get { return _instance; } }
        #endregion
        #region [ Construtor ]
        /// <summary>
        /// 
        /// </summary>
        private ServiceLocator()
        {
            _singleTons = new Dictionary<string, object>();
            _services = new Dictionary<string, object>();
        }
        #endregion

        #region [ Metodos ]
        public void RegisterSingleton<T, T2>() where T2 : class
        {
            var check = typeof(T2).GetInterface(typeof(T).Name);
            if (check == null)
            {
                throw new ApplicationException($"O Singleton \"{typeof(T2).Name}\" deve implementar a interface \"{typeof(T).Name}\" para poder ser registrado.");
            }
            if (_singleTons.Keys.Contains(typeof(T).Name))
            {
                throw new ApplicationException($"Duplicated Singleton Register \"{typeof(T).Name}\"");
            }
            _singleTons.Add(typeof(T).Name, Activator.CreateInstance(typeof(T2)));
        }

        public T GetSingleton<T>()
        {
            try
            {
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
                throw new ApplicationException($"Duplicated Sevices Registred \"{name}\"");
            }
            _services.Add(name, instance);

            (instance as IServiceBase).Configure();
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
                (_services[name] as IServiceBase).Dispose();
                _services.Remove(name);
            }
        }
        #endregion
    }
}
