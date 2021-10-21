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
        /// Metodo que será chamado ao carregar o serviço
        /// </summary>
        void Configure();
        /// <summary>
        /// Metodo que será chamado ao descarregar o serviço
        /// </summary>
        void Dispose();
    }
}
