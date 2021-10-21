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
        /// Se inscreve no Evento passando um metodo de CallBack que deve receber o mesmo tipo do evento
        /// </summary>
        /// <param name="payload">Metodo que será chamado quando ocorrer umas publicação. Deve receber o mesmo tipo do evento cadastrado</param>
        /// <param name="options">Define o tipo de Thread utilizada na chamada do CallBack</param>
        void Subscribe(Action<T> payload, ThreadOptions options = ThreadOptions.MainThread);

        /// <summary>
        /// Remove a inscrição no evento, sendo necessario informar o mesmo Metodo para CallBack utilizado na inscrição
        /// </summary>
        /// <param name="payload">Metodo CallBack utilizado na inscricao</param>
        void Unsubscribe(Action<T> payload);

        /// <summary>
        /// Publica o Objeto do tipo utilizado na criação do evento, chamando todos os Metodos CallBack inscritos
        /// </summary>
        /// <param name="payload"></param>
        void Publish(T payload);
    }

    public interface IPublicEventer
    {
        /// <summary>
        /// Se inscreve no Evento passando um metodo de CallBack
        /// </summary>
        /// <param name="payload">Metodo que será chamado quando ocorrer umas publicação</param>
        /// <param name="options">Define o tipo de Thread utilizada na chamada do CallBack</param>
        void Subscribe(Action payload, ThreadOptions options = ThreadOptions.MainThread);

        /// <summary>
        /// Remove a inscrição no evento, sendo necessario informar o mesmo Metodo para CallBack utilizado na inscrição
        /// </summary>
        /// <param name="payload">Metodo CallBack utilizado na inscricao</param>
        void Unsubscribe(Action payload);

        /// <summary>
        /// Publica o evento, chamando todos os Metodos CallBack inscritos
        /// </summary>
        void Publish();
    }
}
