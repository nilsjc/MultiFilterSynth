namespace SyntCore
{
    public class BandPassFilter
    {
        private int sampleRate;
        private double buf1;
        private double buf0;
        private double frequency;
        private double Q;
        public BandPassFilter(int sampleRate)
        {
            this.sampleRate = sampleRate;
        }
        public void SetFrequency(double f)
        {
            frequency = f;
        }
        public void SetQ(double q)
        {
            Q = q;
        }
        public double Filter(double input)
        {
            buf0 += frequency * (input - buf0 + Q * (buf0 - buf1));
            buf1 += frequency * (buf0 - buf1);
            return buf1;
        }
    }
}
