using CSSUtils.Events;
using CSSUtils.Log;
using CSSUtils.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CSSUtils_Example
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ServiceLocator.Current.RegisterSingleton<IEventer, Eventer>();
            ServiceLocator.Current.RegisterSingleton<ILogger, Logger>();

            ILogger logger = ServiceLocator.Current.GetSingleton<ILogger>();
        }
    }
}
