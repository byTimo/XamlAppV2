using System;
using System.IO;
using NAudio.Mixer;
using NAudio.Wave;
using ZappChat_v3.Core.ChatElements;

namespace ZappChat_v3.Core.Managers
{
    public static class PeripheryManager
    {
        private static WaveIn _inDevice;
        private static WaveOut _outDevice;
        private static ChatMember _interlocutor;

        //public static bool DeviceIsAvalible { get; private set; }
        private static WaveIn RecordingDevice
        {
            get
            {
                if (_inDevice != null) return _inDevice;
                _inDevice = new WaveIn(WaveCallbackInfo.FunctionCallback())
                {
                    DeviceNumber = Settings.Current.InDeviceNumber,
                    WaveFormat = new WaveFormat(44100, WaveIn.GetCapabilities(Settings.Current.InDeviceNumber).Channels)
                };
                return _inDevice;
            }
        }
        private static WaveOut PlayingDevice
        {
            get
            {
                if (_outDevice != null) return _outDevice;
                _outDevice = new WaveOut
                {
                    DeviceNumber = Settings.Current.OutDeviceNumber
                };
                return _outDevice;
            }
        }

        public static MixerLine RecordDeviceMixerLine
        {
            get { return RecordingDevice.GetMixerLine(); }
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
        public static Action<byte[]> CreateTranslation(EventHandler<WaveInEventArgs> sendBayteAction)
        {
            DisposeTranslation();
            RecordingDevice.DataAvailable += sendBayteAction;
            Support.Logger.Info("Send's endpoints connect to P2P manager");
            return PlayByteArray;
        }

        private static void PlayByteArray(byte[] soundInBytes)
        {
            //@TODO Возможно стоит попробовать поменять как то WaveFormat
            var provider = new RawSourceWaveStream(new MemoryStream(soundInBytes), new WaveFormat());
            PlayingDevice.Init(provider);
            PlayingDevice.Play();
        }
        private static void DisposeTranslation()
        {
            _inDevice.Dispose();
            _inDevice = null;
            _outDevice.Dispose();
            _outDevice = null;
            Support.Logger.Info("Resources periphery released");
        }
        
    }
}