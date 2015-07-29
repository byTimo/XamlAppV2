using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private List<MMDevice> InputDevices;
        private List<MMDevice> OutputDevices;
        public MainWindow()
        {
            InitializeComponent();
            InputDevices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            InDeviceComboBox.ItemsSource = InputDevices;
            OutputDevices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
            OutDeviceComboBox.ItemsSource = OutputDevices;
            InDeviceComboBox.SelectedIndex = Settings.Current.InDeviceNumber;
            OutDeviceComboBox.SelectedIndex = Settings.Current.OutDeviceNumber;
        }

        private void InDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Current.InDeviceNumber = InDeviceComboBox.SelectedIndex;
            Settings.Current.InDeviceId = InputDevices[InDeviceComboBox.SelectedIndex].ID;
            CreateTranslation();
        }

        private void OutDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Current.OutDeviceNumber = OutDeviceComboBox.SelectedIndex;
            Settings.Current.OutDeviceId = OutputDevices[Settings.Current.InDeviceNumber].ID;
            CreateTranslation();
        }

        private void CreateTranslation()
        {
            _test =
                PeripheryManager.StartTranslation((sender, args) => _test.Invoke(args.Buffer, 0, args.BytesRecorded));

        }
    }
}
