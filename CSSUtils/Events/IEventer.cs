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
        /// Recupera um Evento previamente cadastrado
        /// </summary>
        /// <typeparam name="T">Nome do Evento</typeparam>
        /// <returns></returns>
        T GetEvent<T>();
    }

    public enum ThreadOptions
    {
        /// <summary>
        /// Executa o CallBack na mesma Thread onde foi Publicada
        /// </summary>
        MainThread,
        /// <summary>
        /// Execute o CallBack na Thread UI
        /// </summary>
        UIThread,
        /// <summary>
        /// Execute o CallBack em uma Thread secundaria
        /// </summary>
        BackgroundThread,
    }
}
