using System;
using System.Windows;
using System.Windows.Controls;
using NAudio.Wave;
using ZappChat_v3.Core;
using ZappChat_v3.Core.Managers;

namespace ZappChat_v3.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Action<byte[], int, int> _test;
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
            _test = PeripheryManager.CreateTranslation((sender, args) => _test.Invoke(args.Buffer, 0, args.BytesRecorded));
        }
    }
}
