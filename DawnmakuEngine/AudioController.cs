using DawnmakuEngine.Data;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DawnmakuEngine
{
    public class AudioController : IDisposable
    {
        private readonly IWavePlayer outputDevice;
        private readonly MixingSampleProvider finalMixer,
            bgmMixer, bulletMixer, bulletSpawnMixer,
            playerShootMixer, playerBulletMixer, playerDeathMixer,
            enemyDeathMixer, miscMixer;
        public int SampleRate { get; private set; }
        public WaveFormat WaveFormat { get; private set; }

        public enum AudioCategory { BGM, Bullet, BulletSpawn, PlayerBulletSpawn, PlayerBullet, Player, Enemy, Misc }
        
        public AudioController(int sampleRate = 44100, int channelcount = 2)
        {
            outputDevice = new WaveOutEvent();
            finalMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            finalMixer.ReadFully = true;
            outputDevice.Init(finalMixer);
            outputDevice.Play();

            bgmMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            bgmMixer.ReadFully = true;

            bulletMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            bulletMixer.ReadFully = true;

            bulletSpawnMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            bulletSpawnMixer.ReadFully = true;

            playerShootMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            playerShootMixer.ReadFully = true;

            playerBulletMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            playerBulletMixer.ReadFully = true;

            playerDeathMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            playerDeathMixer.ReadFully = true;

            enemyDeathMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            enemyDeathMixer.ReadFully = true;

            miscMixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelcount));
            miscMixer.ReadFully = true;



            finalMixer.AddMixerInput(bgmMixer);
            finalMixer.AddMixerInput(playerShootMixer);
            finalMixer.AddMixerInput(playerBulletMixer);
            finalMixer.AddMixerInput(playerDeathMixer);
            finalMixer.AddMixerInput(enemyDeathMixer);
            finalMixer.AddMixerInput(bulletSpawnMixer);
            finalMixer.AddMixerInput(bulletMixer);
            finalMixer.AddMixerInput(miscMixer);

            WaveFormat = finalMixer.WaveFormat;
            SampleRate = sampleRate;
        }

        public void PlaySound(AudioData data, AudioCategory category = AudioCategory.Misc, float volume = 1)
        {
            if(data != null)
                AddMixerInput(CreateProvider(data.ComputeVolume(volume)), category);
        }
        public void PlaySound(string file, AudioCategory category = AudioCategory.Misc, float volume = 1)
        {
            AudioFileReader reader = new AudioFileReader(file);
            reader.Volume = volume;
            AddMixerInput(new AudioData.ReaderProvider(reader), category);
        }

        private ISampleProvider CreateProvider(AudioData data)
        {
            return new AudioData.AudioProvider(data);
        }

        private void AddMixerInput(ISampleProvider provider, AudioCategory category)
        {
            switch(category)
            {
                case AudioCategory.BGM:
                    bgmMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
                case AudioCategory.Bullet:
                    bulletMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
                case AudioCategory.BulletSpawn:
                    bulletSpawnMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
                case AudioCategory.PlayerBulletSpawn:
                    playerShootMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
                case AudioCategory.PlayerBullet:
                    playerBulletMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
                case AudioCategory.Player:
                    playerDeathMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
                case AudioCategory.Enemy:
                    enemyDeathMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
                case AudioCategory.Misc:
                default:
                    miscMixer.AddMixerInput(ConvertChannelCount(provider));
                    break;
            }
            //finalMixer.AddMixerInput(ConvertChannelCount(provider));
        }

        private ISampleProvider ConvertChannelCount(ISampleProvider provider)
        {
            if (provider.WaveFormat.Channels == finalMixer.WaveFormat.Channels)
                return provider;
            if (provider.WaveFormat.Channels == 1 && finalMixer.WaveFormat.Channels == 2)
                return new MonoToStereoSampleProvider(provider);
            GameMaster.LogErrorMessage("Channel count conversion not implemented", "Channel count: " + provider.WaveFormat.Channels);
            return null;
        }

        public void Dispose()
        {
            outputDevice.Dispose();
        }
    }
}
