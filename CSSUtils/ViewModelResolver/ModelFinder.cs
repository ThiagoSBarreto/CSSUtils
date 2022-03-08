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
                throw new ApplicationException($"Page or Window not found: {View.GetType().FullName}");
            }

            object model = Assembly.GetCallingAssembly().CreateInstance(viewModelName);
            if (model != null)
            {
                _viewModels.Add(View, model);
                if (View is Page p) p.DataContext = model;
                else if (View is Window w) w.DataContext = model;
                else
                {
                    throw new ApplicationException($"Couldn't resolve the ViewModel because the class isn't a \"Page\" or a \"Window\"");
                }
            }
            else
            {
                throw new ApplicationException($"ViewModel not found: {viewModelName}");
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
                throw new ApplicationException($"The ViewModel: {_viewModels[sender].GetType().FullName} doesn't have a \"Dispose\" method");
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
                throw new ApplicationException($"The ViewModel: {_viewModels[sender].GetType().FullName} doesn't have a \"Dispose\" method");
            }
        }
    }
}
