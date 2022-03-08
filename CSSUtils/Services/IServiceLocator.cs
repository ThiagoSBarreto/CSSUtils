using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CSSUtils.Services
{
    public interface IServiceLocator
    {
        /// <summary>
        /// Register a Singleton into the ServiceLocator, it must implement it's won interface
        /// </summary>
        /// <typeparam name="Interface">The singleton interface type</typeparam>
        /// <typeparam name="Class">the singleton class type</typeparam>
        void RegisterSingleton<Interface, Class>() where Class : class;

        /// <summary>
        /// Returns the instance of the previcious registered singleton
        /// </summary>
        /// <typeparam name="T">Class type of the Singleton</typeparam>
        /// <returns></returns>
        T GetSingleton<T>();

        /// <summary>
        /// Register a service that must implement the IServiceBase interface and calls "Configurate" and "Start" Methods automatically
        /// </summary>
        /// <typeparam name="Class">The Service class type</typeparam>
        void RegisterService<Class>() where Class : class;

        /// <summary>
        /// Unregister a service and calls "Stop" and "Dispose" Methods automatically
        /// </summary>
        /// <typeparam name="Class">The Service class type</typeparam>
        void UnRegisterService<Class>() where Class : class;

        /// <summary>
        /// Calls the "Stop" Method of the given service
        /// </summary>
        /// <typeparam name="Class">The Service class type</typeparam>
        /// <returns>TRUE if the service STOPED, FALSE Otherwise</returns>
        bool StopService<Class>() where Class : class;

        /// <summary>
        /// Calss the "Start" Method of the given service
        /// </summary>
        /// <typeparam name="Class">The Service class type</typeparam>
        /// <returns>TRUE if the service STARTED, FLASE Ohterwise</returns>
        bool StartService<Class>() where Class : class;
    }
}
