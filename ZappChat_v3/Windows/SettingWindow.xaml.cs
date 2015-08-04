using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private bool mircoOff;
        private bool call;
        public MainWindow()
        {
            InitializeComponent();
            InDeviceComboBox.ItemsSource = PeripheryManager.CaptureDevicesCollection;
            OutDeviceComboBox.ItemsSource = PeripheryManager.RenderDevicesCollection;
            InDeviceComboBox.SelectedIndex = Settings.Current.InDeviceNumber;
            OutDeviceComboBox.SelectedIndex = Settings.Current.OutDeviceNumber;
        }

        private void InDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Current.InDeviceNumber = InDeviceComboBox.SelectedIndex;
            Settings.Current.InDeviceId = PeripheryManager.GetCapturedDeviceId(InDeviceComboBox.SelectedIndex);
        }

        private void OutDeviceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.Current.OutDeviceNumber = OutDeviceComboBox.SelectedIndex;
            Settings.Current.OutDeviceId = PeripheryManager.GetRenderDeviceId(OutDeviceComboBox.SelectedIndex);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!mircoOff)
            {
                Micro.Content = "Micro ON";
                PeripheryManager.StopCaptureInputeWave();
            }
            else
            {
                Micro.Content = "Micro OFF";
                PeripheryManager.StartCaptureInputeWave();
            }
            mircoOff = !mircoOff;
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            if (!call)
            {
                CallButton.Content = "Break";
                _test = PeripheryManager.BindingTranslation((o, args) => _test.Invoke(args.Buffer, 0, args.BytesRecorded));
                PeripheryManager.StartTranslation();
            }
            else
            {
                CallButton.Content = "Call";
                PeripheryManager.StopTranslation();
            }
            call = !call;
        }
    }
}
