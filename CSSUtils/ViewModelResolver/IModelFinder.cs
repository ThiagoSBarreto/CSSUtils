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
        /// Resolves the ViewModel for a Page or Window, Binding the DataContexts
        /// It will also attach a event to Page.Unloaded or Window.Closing that will call the Method Dispose on the ViewModel automatically.
        /// The viewModel must inherit the BaseViewModel class
        /// The viewModel must have a Public Dispose Method
        /// </summary>
        /// <param name="View">The Page or Window object</param>
        void Resolve(object View);
    }
}
