using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace ZappChat_v3.Core.Managers
{
    public static class PeripheryManager
    {
        private static IWaveIn _waveIn;
        private static IWavePlayer _waveOut;
        private static BufferedWaveProvider _waveProvider;

        static PeripheryManager()
        {
            Settings.SettingsChanged += DisposeWave;
        }

#region private properties
        private static IWaveIn WaveIn
        {
            get
            {
                if (_waveIn != null) return _waveIn;
                _waveIn = new WasapiCapture(CapturedDevice);
                return _waveIn;
            }
        }
        private static IWavePlayer WaveOut
        {
            get
            {
                if (_waveOut != null) return _waveOut;
                _waveOut = new WasapiOut(RenderDevice, AudioClientShareMode.Shared, false, 300);
                return _waveOut;
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
        public static List<MMDevice> CaptureDevicesCollection => new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();

        /// <summary>
        /// Получить коллекцию всех активных устройств вывода звука
        /// </summary>
        public static List<MMDevice> RenderDevicesCollection => new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();

        /// <summary>
        /// Метод назначает прямой вывод захваченых байт выводящему устройству
        /// </summary>
        public static void BindingTranslation()
        {
            StopTranslation();
            var provider = new WaveInProvider(WaveIn);
            WaveOut.Init(provider);
            WaveIn.StartRecording();
            WaveOut.Play();
            Support.Logger.Info("Self-translation created");
        }
        //@TODO - проверим, когда будет менеджер P2P
        /// <summary>
        /// Метод назначает передачу захваченного массива байт входному делегату и возвращает делегат, которому назначается воспроизведение полученого массива
        /// </summary>
        /// <param name="sendBayteAction">Делегат, работающий с захваченным массивом байт</param>
        /// <returns>Делегат, вызов которго воспроизводит переданный массив байт</returns>
        public static Action<byte[], int, int> BindingTranslation(EventHandler<WaveInEventArgs> sendBayteAction)
        {
            StopTranslation();

            WaveIn.DataAvailable += (sender, args) =>
            {
                var encodedBytes = args.Buffer;//Без кодирования
                sendBayteAction.Invoke(sender, new WaveInEventArgs(encodedBytes, args.BytesRecorded));
            };
            WaveIn.StartRecording();
            Support.Logger.Info("Send's endpoints connect to P2P manager");

            _waveProvider = new BufferedWaveProvider(WaveIn.WaveFormat /*Без кодирования*/);
            WaveOut.Init(_waveProvider);
            WaveOut.Play();
            return PlayByteArray;
        }
        /// <summary>
        /// Метод начинает трансляцию по текущему биндуы
        /// </summary>
        public static void StartTranslation()
        {
            StartCaptureInputeWave();
            if (_waveOut != null)
                WaveOut.Play();
        }
        /// <summary>
        /// Метод останавливает текущую трансляцию, если она активна
        /// </summary>
        public static void StopTranslation()
        {
            DisposeWave();
            Support.Logger.Info("Translation is stoped successfully");
        }
        /// <summary>
        /// Метод включает захват звука микрофоном
        /// </summary>
        public static void StartCaptureInputeWave()
        {
            if(_waveIn != null)
                WaveIn.StartRecording();
        }
        /// <summary>
        /// Метод отключает захват звука микрофоном
        /// </summary>
        public static void StopCaptureInputeWave()
        {
            if(_waveIn != null)
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
            _waveProvider.AddSamples(decodedBytes, offset, count);
        }
        private static void DisposeWave()
        {
            if (_waveIn != null)
            {
                _waveIn.StopRecording();
                _waveIn.Dispose();
                _waveIn = null;
            }
            if (_waveOut != null)
            {
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
            if (_waveProvider != null)
            {
                _waveProvider.ClearBuffer();
                _waveProvider = null;
            }
            Support.Logger.Trace("Resources periphery released");
        }
    }
}