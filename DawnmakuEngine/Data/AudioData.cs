using System;
using System.Collections.Generic;
using System.Linq;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace DawnmakuEngine.Data
{
    public class AudioData
    {
        public float[] audioData;
        public WaveFormat format;

        public AudioData (string file)
        {
            AudioFileReader reader = new AudioFileReader(file);
            format = reader.WaveFormat;
            List<float> fileData = new List<float>((int)(reader.Length / 4));
            List<byte> fileDataByte = new List<byte>((int)reader.Length);
            float[] readBuffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];
            byte[] readBufferByte = new byte[reader.Length];
            int samplesRead = 0;

            if(GameMaster.gameMaster.audioManager != null)
            {
                ResamplerDmoStream otherResampler = new ResamplerDmoStream(reader, GameMaster.gameMaster.audioManager.WaveFormat);
                otherResampler.ToSampleProvider();
                while ((samplesRead = otherResampler.Read(readBufferByte, 0, readBufferByte.Length)) > 0)
                {
                    fileDataByte.AddRange(readBufferByte.Take(samplesRead));
                }
                readBufferByte = fileDataByte.ToArray();
                fileData = new List<float>((int)(readBufferByte.Length / 4));
                for (int i = 0; i < fileDataByte.Count / 4; i++)
                    fileData.Add(BitConverter.ToSingle(readBufferByte, i * 4));
                format = otherResampler.WaveFormat;
            }
            else
            {
                while ((samplesRead = reader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    fileData.AddRange(readBuffer.Take(samplesRead));
                }
            }



            audioData = fileData.ToArray();

            reader.Dispose();
        }
        public AudioData(float[] readData, WaveFormat readFormat)
        {
            audioData = (float[])readData.Clone();
            format = readFormat;
        }

        public AudioData ComputeVolume(float volume)
        {
            AudioData output = new AudioData(audioData, format);
            for (int i = 0; i < output.audioData.Length; i++)
                output.audioData[i] *= volume;
            return output;
        }

        public class AudioProvider : ISampleProvider
        {
            private readonly AudioData data;
            private long position;
            public AudioProvider(AudioData newData) { data = newData; }

            public WaveFormat WaveFormat { get { return data.format; } }
            public int Read(float[] buffer, int offset, int count)
            {
                long availableSamples = data.audioData.Length - position;
                long samplesToCopy = Math.Min(availableSamples, count);
                Array.Copy(data.audioData, position, buffer, offset, samplesToCopy);
                position += samplesToCopy;
                return (int)samplesToCopy;
            }
        }

        public class ReaderProvider : ISampleProvider
        {
            private readonly AudioFileReader reader;
            private bool isDisposed;
            
            public ReaderProvider(AudioFileReader newReader)
            {
                reader = newReader;
                WaveFormat = newReader.WaveFormat;
            }

            public WaveFormat WaveFormat { get; set; }
            public int Read(float[] buffer, int offset, int count)
            {
                if (isDisposed)
                    return 0;
                int read = reader.Read(buffer, offset, count);
                if (read == 0)
                {
                    reader.Dispose();
                    isDisposed = true;
                }
                return read;
            }
        }
    }
}
