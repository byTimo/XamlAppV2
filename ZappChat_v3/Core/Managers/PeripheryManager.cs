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
        }
        private static void DisposeTranslation()
        {
            _inDevice.Dispose();
            _inDevice = null;
            _outDevice.Dispose();
            _outDevice = null;
        }
        
    }
}