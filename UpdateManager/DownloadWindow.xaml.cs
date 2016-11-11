using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UpdateManager.Core;
using UpdateManager.Entity;

namespace UpdateManager
{
    /// <summary>
    /// Interaction logic for DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window
    {
        List<ProgressBar> progressBarsList = new List<ProgressBar>();
        List<DataGridEntity> driverList = new List<DataGridEntity>();

        public DownloadWindow()
        {
            InitializeComponent();
        }

        public DownloadWindow(List<DataGridEntity> list)
        {
            InitializeComponent();
            driverList = list;
            gridWithElements.ColumnDefinitions.Add(new ColumnDefinition());
            gridWithElements.ColumnDefinitions.Add(new ColumnDefinition());
            for (var i = 0; i < list.Count; i++)
            {
                gridWithElements.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                var label = new Label();
                label.Width = 381;
                label.Content = list[i].driver.device;

                var progress = new ProgressBar();
                progress.Height = 15;
                progress.Width = 150;
                Grid.SetColumn(label, 0);
                Grid.SetColumn(progress, 1);
                progressBarsList.Add(progress);

                Grid.SetRow(label, i);
                Grid.SetRow(progress, i);
                gridWithElements.Children.Add(label);
                gridWithElements.Children.Add(progress);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkWithDriver.downloadDrivers(driverList, progressBarsList);
        }
    }
}
