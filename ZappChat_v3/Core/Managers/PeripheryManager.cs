using System;
using System.IO;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace ZappChat_v3.Core.Managers
{
    public static class PeripheryManager
    {
        private static IWaveIn inDevice;
        private static IWavePlayer outDevice;
        private static BufferedWaveProvider waveProvider;

        private static IWaveIn WaveIn
        {
            get
            {
                if (inDevice != null) return inDevice;
                inDevice = new WasapiCapture(CapturedDevice);
                return inDevice;
            }
        }
        private static IWavePlayer WaveOut
        {
            get
            {
                if (outDevice != null) return outDevice;
                outDevice = new WasapiOut(RenderDevice, AudioClientShareMode.Shared, false, 300);
                return outDevice;
            }
        }

        private static MMDevice CapturedDevice
        {
            get
            {
                return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[
                        Settings.Current.InDeviceNumber];
            }
        }

        private static MMDevice RenderDevice
        {
            get
            {
                return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[
                        Settings.Current.OutDeviceNumber];
            }
        }

        /// <summary>
        /// Метод создаёт трансляцию, в которой захваченые байты передаются сразу на устройство вывода.
        /// </summary>
        public static void CreateTranslation()
        {
            DisposeTranslation();

            var provider = new WaveInProvider(WaveIn);
            WaveOut.Init(provider);
            WaveIn.StartRecording();
            WaveOut.Play();
            Support.Logger.Info("Self-translation created");
        }
        //@TEST - проверим, когда будет менеджер P2P
        public static Action<byte[], int, int> CreateTranslation(EventHandler<WaveInEventArgs> sendBayteAction)
        {
            DisposeTranslation();

            WaveIn.DataAvailable += (sender, args) =>
            {
                var encodedBytes = args.Buffer;//Без кодирования
                sendBayteAction.Invoke(sender, new WaveInEventArgs(encodedBytes, args.BytesRecorded));
            };
            WaveIn.StartRecording();
            Support.Logger.Info("Send's endpoints connect to P2P manager");

            waveProvider = new BufferedWaveProvider(WaveIn.WaveFormat /*Без кодирования*/);
            WaveOut.Init(waveProvider);
            WaveOut.Play();
            return PlayByteArray;
        }

        /// <summary>
        /// Метод освобождает все ресурсы менеджера
        /// </summary>
        public static void FinalizePeriphery()
        {
            DisposeTranslation();
            Support.Logger.Info("Successful dispose all periphery manager's recourses");
        }

        private static void PlayByteArray(byte[] buffer, int offset, int count)
        {
            var decodedBytes = buffer;//Без кодирования
            waveProvider.AddSamples(decodedBytes, offset, count);
        }
        private static void DisposeTranslation()
        {
            if (inDevice != null)
            {
                inDevice.StopRecording();
                inDevice.Dispose();
                inDevice = null;
            }
            if (outDevice != null)
            {
                outDevice.Stop();
                outDevice.Dispose();
                outDevice = null;
            }
            if (waveProvider != null)
            {
                waveProvider.ClearBuffer();
                waveProvider = null;
            }
            Support.Logger.Info("Resources periphery released");
        }
    }
}