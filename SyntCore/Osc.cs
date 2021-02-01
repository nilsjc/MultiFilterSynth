using System;
using System.Collections.Generic;
using System.Text;

namespace SyntCore
{
    public class Osc
    {
        private int sampleRate;
        private double phase;
        private double targetPhaseStep;
        private int waveTableLength;
        private int waveTableIndex;
        public bool Gate { get; set; }
        public Osc(int sampleRate, int waveTableLength)
        {
            this.sampleRate = sampleRate;
            this.waveTableLength = waveTableLength;
        }
        public void SetFreq(double frequency)
        {
            targetPhaseStep = sampleRate * (frequency / sampleRate);
        }
        public int Tick()
        {
            waveTableIndex = (int)phase % waveTableLength;
            phase += targetPhaseStep;
            if (phase > sampleRate)
                phase -= sampleRate;
            return waveTableIndex;
        }
    }
}
