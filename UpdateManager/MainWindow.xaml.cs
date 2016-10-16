using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Management;
using System.Net;
using UpdateManager.Core;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace UpdateManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            WorkWithDriver.getDriversForUpdate();
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
