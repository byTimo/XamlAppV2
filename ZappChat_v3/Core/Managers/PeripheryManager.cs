using System;
using NAudio.Wave;

namespace ZappChat_v3.Core.Managers
{
    public static class PeripheryManager
    {
        private static WaveIn inDevice;
        private static WaveOut outDevice;
        private static BufferedWaveProvider waveProvider;
        //private static ChatMember _interlocutor;

        //public static bool DeviceIsAvalible { get; private set; }
        private static WaveIn RecordingDevice
        {
            get
            {
                if (inDevice != null) return inDevice;
                inDevice = new WaveIn(WaveCallbackInfo.FunctionCallback())
                {
                    DeviceNumber = Settings.Current.InDeviceNumber,
                    WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Settings.Current.InDeviceNumber).Channels)
                };
                return inDevice;
            }
        }
        private static WaveOut PlayingDevice
        {
            get
            {
                if (outDevice != null) return outDevice;
                outDevice = new WaveOut
                {
                    DeviceNumber = Settings.Current.OutDeviceNumber
                };
                return outDevice;
            }
        }

        /// <summary>
        /// Метод создаёт трансляцию, в которой захваченые байты передаются сразу на устройство вывода.
        /// </summary>
        public static void CreateTranslation()
        {
            DisposeTranslation();

            var provider = new WaveInProvider(RecordingDevice);
            PlayingDevice.Init(provider);
            RecordingDevice.StartRecording();
            PlayingDevice.Play();
            Support.Logger.Info("Self-translation created");
        }
        //@TODO метод создания трансляции на основе двух делегатов 1 - подписывается на DataAvailable микрофона
        //@TODO 2 - каким то образом определеяет откуда брать поток байт для пеера. Либо передать 
        //@TEST - проверим, когда будет менеджер P2P
        public static Action<byte[], int, int> CreateTranslation(EventHandler<WaveInEventArgs> sendBayteAction)
        {
            DisposeTranslation();

            RecordingDevice.DataAvailable += sendBayteAction;
            RecordingDevice.StartRecording();
            Support.Logger.Info("Send's endpoints connect to P2P manager");
            //@TODO попробовать другие IWaveFormat
            waveProvider = new BufferedWaveProvider(new WaveFormat());
            PlayingDevice.Init(waveProvider);
            PlayingDevice.Play();
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
            waveProvider.AddSamples(buffer, offset, count);
        }
        private static void DisposeTranslation()
        {
            if (inDevice != null)
            {
                inDevice.Dispose();
                inDevice = null;
            }
            if (outDevice != null)
            {
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