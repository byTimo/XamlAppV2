using System;
using System.Windows;
using System.Windows.Controls;
using NAudio.CoreAudioApi;
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
            var inputDevices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            foreach (var inputDevice in inputDevices)
            {
                InDeviceComboBox.Items.Add(inputDevice.ToString());
            }
            var outputDevices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            foreach (var outputDevice in outputDevices)
            {
                OutDeviceComboBox.Items.Add(outputDevice.ToString());
            }
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
            _test =
                PeripheryManager.CreateTranslation((sender, args) => _test.Invoke(args.Buffer, 0, args.BytesRecorded));

        }
    }
}
