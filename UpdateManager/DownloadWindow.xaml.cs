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
        List<Label> labelList = new List<Label>();

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
                label.Content = list[i].driver.deviceName;

                var statusLabel = new Label();
                statusLabel.Width = 150;
                statusLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                labelList.Add(statusLabel);

                var progress = new ProgressBar();
                progress.Height = 15;
                progress.Width = 150;               
                progressBarsList.Add(progress);

                Grid.SetColumn(label, 0);
                Grid.SetRow(label, i);
                Grid.SetColumn(statusLabel, 1);
                Grid.SetRow(statusLabel, i);
                Grid.SetColumn(progress, 1);                
                Grid.SetRow(progress, i);

                gridWithElements.Children.Add(label);              
                gridWithElements.Children.Add(progress);
                gridWithElements.Children.Add(statusLabel);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkWithDriver.updateDriversFull(driverList, progressBarsList, labelList);
        }
    }
}
