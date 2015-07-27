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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using ZappChat_v3.Core;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Action<byte[]> peripheryManagerCallBack; 
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
                InDeviceComboBox.Items.Add(WaveIn.GetCapabilities(i).ProductName);
            for (int i = 0; i < WaveOut.DeviceCount; i++)
                OutDeviceComboBox.Items.Add(WaveOut.GetCapabilities(i).ProductName);
            InDeviceComboBox.SelectedIndex = 0;
            OutDeviceComboBox.SelectedIndex = 0;
        }

        private void InDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Current.InDeviceNumber = InDeviceComboBox.SelectedIndex;
            CreateTranslation();
        }

        private void OutDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Current.OutDeviceNumber = OutDeviceComboBox.SelectedIndex;
            CreateTranslation();
        }

        private void CreateTranslation()
        {
            peripheryManagerCallBack = PeripheryManager.CreateTranslation((sender, args) =>
            {
                Test.Text = Encoding.UTF8.GetString(args.Buffer);
            });
        }
    }
}
