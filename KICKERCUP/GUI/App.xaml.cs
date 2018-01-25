using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Logic.Database;

namespace GUI
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public void ApplicationStartup(object sender, StartupEventArgs e)
        {
            ConnectionTool.SetValue("SetDBConnectionString", false);
            ConnectionTool.SetValue("DBConnectionString", null);
        }
    }
}