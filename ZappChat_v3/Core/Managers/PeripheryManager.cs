using System;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace ZappChat_v3.Core.Managers
{
    public static class PeripheryManager
    {
        private static WaveIn inDevice;
        private static WaveOut outDevice;
        private static BufferedWaveProvider waveProvider;

        private static MMDevice cupturDevice;
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
                    WaveFormat = ALawCodec.RecordFormat,
                    BufferMilliseconds = 50
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

            RecordingDevice.DataAvailable += (sender, args) =>
            {
                var encodedBytes = ALawCodec.Encode(args.Buffer, 0, args.BytesRecorded);
                sendBayteAction.Invoke(sender, new WaveInEventArgs(encodedBytes, args.BytesRecorded));
            };
            RecordingDevice.StartRecording();
            Support.Logger.Info("Send's endpoints connect to P2P manager");
            waveProvider = new BufferedWaveProvider(ALawCodec.RecordFormat);
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
            var decodedBytes = ALawCodec.Decode(buffer, 0, buffer.Length);
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