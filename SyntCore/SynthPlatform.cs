using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyntCore
{
    /// <summary>
    /// Instance of audioprovider and sequencer
    /// </summary>
    public class SynthPlatform
    {
        const int SampleRate = 44100;
        AudioProvider _audioProvider;
        WaveOutEvent _waveOutEvent;
        IWavePlayer _player;
        SampleToWaveProvider _sampToWaveProvider;
        public SynthPlatform()
        {
            _audioProvider = new AudioProvider(sampleRate:SampleRate);
            _waveOutEvent = new WaveOutEvent
            {
                NumberOfBuffers = 2,
                DesiredLatency = 100
            };
            _player = _waveOutEvent;
            _sampToWaveProvider = new SampleToWaveProvider(_audioProvider);
            _player.Init(_sampToWaveProvider);
        }
        /// <summary>
        /// Start aúdio engine
        /// </summary>
        public void Start()
        {
            _player.Play();
        }
        /// <summary>
        /// Stop audio engine
        /// </summary>
        public void Stop()
        {
            _player.Stop();
        }
        /// <summary>
        /// Set frequency of oscillators
        /// </summary>
        /// <param name="channel">Select oscillator</param>
        /// <param name="frequency">Set frequency</param>
        public void SetFrequency(int channel, int frequency)
        {
            _audioProvider.SetFreq(frequency, channel);
        }
        /// <summary>
        /// Sets destination of oscillator
        /// </summary>
        /// <param name="channel">Select oscillator</param>
        /// <param name="dest">Set destination bit row</param>
        public void SetDestination(int channel, int dest)
        {
            _audioProvider.SetDest(channel, dest);
        }

        public int GetDestinationMatrix(int oscillator)
        {
            return _audioProvider.GetDestMatrix(oscillator);
        }
        public void SetVolOfOsc(int oscillator, float vol)
        {
            _audioProvider.SetVol(vol, oscillator);
        }
        public void SetTempo(int tempo)
        {
            _audioProvider.SetTempo(tempo);
        }
    }
}
