using System;
using System.Collections.Generic;
using System.Text;

namespace SyntCore
{
    public class Resonator
    {
        double z1, z2;
        double a0, a1, a2, b1, b2;
        public Resonator(int sampleRate, double Q, double freq)
        {
            a0 = 1.0;
            a1 = a2 = b1 = b2 = 0.0;
            z1 = z2 = 0.0;

            freq /= sampleRate;
            double K = Math.Tan(Math.PI * freq);
            double norm = 1 / (1 + K / Q + K * K);
            a0 = K / Q * norm;
            a1 = 0;
            a2 = -a0;
            b1 = 2 * (K * K - 1) * norm;
            b2 = (1 - K / Q + K * K) * norm;
        }
        public double Filter(double signal)
        {
            return Process(signal);
        }

        private double Process(double input)
        {
            double output = input * a0 + z1;
            z1 = input * a1 + z2 - b1 * output;
            z2 = input * a2 - b2 * output;
            return output;
        }
    }
}
