using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyntCore
{
    public class AudioProvider : ISampleProvider
    {
        const int NumberOfOscillators = 8;
        const int NumberOfAudioChannels = 1;
        const int SampleRate = 44100;

        private Osc[] Oscillators;
        private int[] OscillatorDestination;
        private double[] Destinations;
        private double FreqMod;
        private Resonator resonator1;
        private Resonator resonator2;
        private Resonator resonator3;
        private Resonator resonator4;
        private Resonator resonator5;
        private Resonator resonator6;
        private Resonator resonator7;
        private Resonator resonator8;
        private Sequencer sequencer;

        private float[] Levels;
        private float[] Wavetable = new float[SampleRate];
        public AudioProvider()
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, NumberOfAudioChannels);
            InitModules();
        }
        public AudioProvider(int sampleRate)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, NumberOfAudioChannels);
            InitModules();
        }
        public void SetFreq(double freq, int channel)
        {
            if(channel < NumberOfOscillators && channel > -1)
            {
                Oscillators[channel].SetFreq(freq);
            }
        }
        public void SetVol(float vol, int channel)
        {
            if (channel < NumberOfOscillators && channel > -1)
            {
                Levels[channel] = vol;
            }
        }
        public void SetDest(int oscillator, int dest)
        {
            OscillatorDestination[oscillator] = dest;
        }
        public void SetTempo(int tempo)
        {
            sequencer.Tempo = tempo;
        }
        public int GetDestMatrix(int oscillator)
        {
            return OscillatorDestination[oscillator];
        }
        public WaveFormat WaveFormat { get; private set; }
        
        
        public int Read(float[] buffer, int offset, int count)
        {
            for (int n = 0; n < count; ++n)
            {
                sequencer.Tick();
                bool freqMod = false;
                int mask;
                int index;
                for (int z = 0; z < 8; z++)
                {
                    Destinations[z] = 0;
                }
                for (int i = 0; i < NumberOfOscillators; i++)
                {
                    index = Oscillators[i].Tick();
                    float oscOut = Wavetable[index] * Levels[i];
                    oscOut *= sequencer.ReturnValue[i];
                    for (int y = 0; y < 8; y++)
                    {
                        mask = OscillatorDestination[i];
                        mask >>= y;
                        mask &= 1;
                        if (mask == 1)
                            {
                                Destinations[y] += oscOut * 0.8;//Levels[i];
                            }
                        }
                }
                var raw = Destinations[0];
                var filter1 = resonator1.Filter(Destinations[1]);
                var filter2 = resonator2.Filter(Destinations[2]);
                var filter3 = resonator3.Filter(Destinations[3]);
                var filter4 = resonator4.Filter(Destinations[4]);
                var filter5 = resonator5.Filter(Destinations[5]);
                var filter6 = resonator6.Filter(Destinations[6]);
                var filter7 = resonator7.Filter(Destinations[7]);
                
                //FreqMod = Destinations[4];

                var mixed = raw + filter1 + filter2 + filter3 + filter4 + filter5 + filter6 + filter7;
                float floatMixed = (float)mixed;

                buffer[n + offset] = floatMixed;//sum;
            }
            return count;   
        }
        private void InitModules()
        {
            sequencer = new Sequencer(SampleRate);

            resonator1 = new Resonator(SampleRate, 20.0, 50);
            resonator2 = new Resonator(SampleRate, 20.0, 100);
            resonator3 = new Resonator(SampleRate, 20.0, 200);
            resonator4 = new Resonator(SampleRate, 20.0, 400);
            resonator5 = new Resonator(SampleRate, 20.0, 800);
            resonator6 = new Resonator(SampleRate, 20.0, 1600);
            resonator7 = new Resonator(SampleRate, 20.0, 3200);
            resonator8 = new Resonator(SampleRate, 20.0, 6400);

            Levels = new float[8];
            Oscillators = new Osc[NumberOfOscillators];
            for (int i = 0; i < NumberOfOscillators; i++)
            {
                Oscillators[i] = new Osc(SampleRate, (Wavetable.Length - 1));
            }
            for (int index = 0; index < SampleRate; ++index)
                Wavetable[index] = index < SampleRate / 6 ? 1 : 0;
            //Wavetable[index] = (float)index / SampleFrequency;
            //Wavetable[index] = (float)Math.Sin(2 * Math.PI * index / SampleFrequency);
            OscillatorDestination = new int[8];
            Destinations = new double[8];
        }
    }
}
