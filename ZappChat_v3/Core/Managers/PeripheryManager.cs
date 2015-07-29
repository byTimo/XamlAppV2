using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace ZappChat_v3.Core.Managers
{
    public static class PeripheryManager
    {
        private static IWaveIn waveIn;
        private static IWavePlayer waveOut;
        private static BufferedWaveProvider waveProvider;

        static PeripheryManager()
        {
            Settings.SettingsChanged += DisposeWave;
        }

#region private properties
        private static IWaveIn WaveIn
        {
            get
            {
                if (waveIn != null) return waveIn;
                waveIn = new WasapiCapture(CapturedDevice);
                return waveIn;
            }
        }
        private static IWavePlayer WaveOut
        {
            get
            {
                if (waveOut != null) return waveOut;
                waveOut = new WasapiOut(RenderDevice, AudioClientShareMode.Shared, false, 300);
                return waveOut;
            }
        }

        private static MMDevice CapturedDevice
        {
            get
            {
                if (Settings.Current.InDeviceId != null)
                    return new MMDeviceEnumerator().GetDevice(Settings.Current.InDeviceId);
                return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active)[
                        Settings.Current.InDeviceNumber];
            }
        }

        private static MMDevice RenderDevice
        {
            get
            {
                if (Settings.Current.OutDeviceId != null)
                    return new MMDeviceEnumerator().GetDevice(Settings.Current.OutDeviceId);
                return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)[
                        Settings.Current.OutDeviceNumber];
            }
        }
#endregion
        /// <summary>
        /// Получить коллекцию всех активных устройств захвата звука
        /// </summary>
        public static List<MMDevice> CaptureDevicesCollection
        {
            get
            {
                return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();
            }
        }
        /// <summary>
        /// Получить коллекцию всех активных устройств вывода звука
        /// </summary>
        public static List<MMDevice> RenderDevicesCollection
        {
            get
            {
                return new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
            }
        }

        /// <summary>
        /// Метод начинает трансляцию, в которой захваченые байты передаются сразу на устройство вывода.
        /// </summary>
        public static void StartTranslation()
        {
            StopTranslation();
            var provider = new WaveInProvider(WaveIn);
            WaveOut.Init(provider);
            WaveIn.StartRecording();
            WaveOut.Play();
            Support.Logger.Info("Self-translation created");
        }
        //@TODO - проверим, когда будет менеджер P2P
        public static Action<byte[], int, int> StartTranslation(EventHandler<WaveInEventArgs> sendBayteAction)
        {
            StopTranslation();

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
        /// Метод останавливает текущую трансляцию, если она активна
        /// </summary>
        public static void StopTranslation()
        {
            /*if(waveIn != null)
                WaveIn.StopRecording();
            if(waveOut != null)
                WaveOut.Stop();
            if(waveProvider != null)
                waveProvider.ClearBuffer();*/
            DisposeWave();
            Support.Logger.Info("Translation is stoped successfully");
        }
        /// <summary>
        /// Метод включает захват звука микрофоном
        /// </summary>
        public static void StartCaptureInputeWave()
        {
            if(waveIn != null)
                WaveIn.StartRecording();
        }
        /// <summary>
        /// Метод отключает захват звука микрофоном
        /// </summary>
        public static void StopCaptureInputeWave()
        {
            if(waveIn != null)
                WaveIn.StopRecording();
        }
        /// <summary>
        /// Метод освобождает все ресурсы менеджера
        /// </summary>
        public static void FinalizePeriphery()
        {
            DisposeWave();
            Support.Logger.Info("Successfull dispose all periphery manager's recourses");
        }
        /// <summary>
        /// Метод возвращает ID устройства захвата по его номеру
        /// </summary>
        /// <param name="deviceNumber">Порядковый номер устройства захвата</param>
        /// <returns>Идентификатор устройства</returns>
        public static string GetCapturedDeviceId(int deviceNumber)
        {
            return CaptureDevicesCollection[deviceNumber].ID;
        }
        /// <summary>
        /// Метод возвращает ID устройства вывода по его номеру
        /// </summary>
        /// <param name="deviceNumber">Порядковый номер устройства вывода</param>
        /// <returns>Идентификатор устройства</returns>
        public static string GetRenderDeviceId(int deviceNumber)
        {
            return RenderDevicesCollection[deviceNumber].ID;
        }

        private static void PlayByteArray(byte[] buffer, int offset, int count)
        {
            var decodedBytes = buffer;//Без кодирования
            waveProvider.AddSamples(decodedBytes, offset, count);
        }
        private static void DisposeWave()
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
            }
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
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