using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SESSIONWPF
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static string ServerName = "DESKTOP-GSTE41V\\MPTSERVER";
        private static string login = "sa";
        private static string password = "123";
        public static string ConnectionString = $@"Data Source = {ServerName}; Initial Catalog = Confectionery; Persist Security Info = true; User ID = {login}; Password = '{password}';";

        public static string loginPeson;

        
    }
}
