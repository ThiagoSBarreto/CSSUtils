using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CSSUtils.ViewModelResolver
{
    public class ModelFinder : IModelFinder
    {
        private static Dictionary<object, object> _viewModels = new Dictionary<object, object>();

        public void Resolve(object View)
        {
            string viewModelName = "";
            Type type = View.GetType();
            if (View is Page page)
            {
                viewModelName = $"{type.FullName}Model";
                page.Unloaded += DisposePage;
            }
            else if (View is Window window)
            {
                viewModelName = $"{type.FullName}Model";
                window.Closing += DisposeWindow;
            }
            else
            {
                throw new ApplicationException($"Pagina ou Janela não encontrada: {View.GetType().FullName}");
            }

            object model = Assembly.GetCallingAssembly().CreateInstance(viewModelName);
            if (model != null)
            {
                _viewModels.Add(View, model);
                if (View is Page p) p.DataContext = model;
                else if (View is Window w) w.DataContext = model;
                else
                {
                    throw new ApplicationException($"Não foi possivel resolver o ViewModel porque a classe utilizado não é uma \"Page\" ou \"Window\"");
                }
            }
            else
            {
                throw new ApplicationException($"Não foi encontrado o ViewModel correspondente: {viewModelName}");
            }
        }

        private void DisposePage(object sender, RoutedEventArgs e)
        {
            try
            {
                _viewModels[sender].GetType().GetMethod("Dispose").Invoke(_viewModels[sender], new object[0]);
                _viewModels.Remove(sender);
            }
            catch
            {
                throw new ApplicationException($"O ViewModel: {_viewModels[sender].GetType().FullName} não contém o metódo \"Dispose\"");
            }
        }

        private void DisposeWindow(object sender, CancelEventArgs e)
        {
            try
            {
                _viewModels[sender].GetType().GetMethod("Dispose").Invoke(_viewModels[sender], new object[0]);
            }
            catch
            {
                Console.Error.WriteLine($"O ViewModel {_viewModels[sender].GetType().FullName} não contém o metódo \"Dispose\"");
            }
        }
    }
}
