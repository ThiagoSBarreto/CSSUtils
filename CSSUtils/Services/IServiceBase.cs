using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.Services
{
    public interface IServiceBase
    {
        /// <summary>
        /// Method that will be called when the Service is Registered
        /// </summary>
        void Configure();
        /// <summary>
        /// Method that will be called when the Service is Stoped
        /// </summary>
        void Dispose();

        /// <summary>
        /// Method that will stop the service
        /// </summary>
        void Stop();

        /// <summary>
        /// Method that will start the service
        /// </summary>
        void Start();
    }
}
