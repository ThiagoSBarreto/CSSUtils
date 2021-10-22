using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSUtils.ViewModelResolver
{
    public interface IModelFinder
    {
        /// <summary>
        /// <para>Encontra, Instancia e Coloca o ViewModel apropriado para esta classe como o DataContext</para>
        /// <para>Deve seguir a seguinte regra: View: PageView / ViewModel: PageViewModel</para>
        /// <para>O ViewModel deve conter o metodo Dispose que será chamado automaticamente</para>
        /// <para>quando a Window ou Page é descarregado, o metodo Dispose é chamado automaticamente</para>
        /// </summary>
        /// <param name="View">"this" a Instancia atual da Classe da View</param>
        void Resolve(object View);
    }
}
