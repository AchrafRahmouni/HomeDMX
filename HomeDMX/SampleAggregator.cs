using System;
using NAudio.Dsp;

namespace HomeDMX
{
    class SampleAggregator
    {
        // FFT
        public event EventHandler<FftEventArgs> FftCalculated;
        public bool PerformFft { get; set; }

        // This Complex is NAudio's own! 
        private readonly Complex[] _fftBuffer;
        private readonly FftEventArgs _fftArgs;
        private int _fftPos;
        private readonly int _fftLength;
        private readonly int _m;

        public SampleAggregator(int fftLength)
        {
            if (!IsPowerOfTwo(fftLength))
            {
                throw new ArgumentException("FFT Length must be a power of two");
            }
            _m = (int)Math.Log(fftLength, 2.0);
            _fftLength = fftLength;
            _fftBuffer = new Complex[fftLength];
            _fftArgs = new FftEventArgs(_fftBuffer);
        }

        private static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public void Add(float value)
        {
            if (!PerformFft || FftCalculated == null)
                return;

            // Remember the window function! There are many others as well.
            _fftBuffer[_fftPos].X = (float)(value * FastFourierTransform.HammingWindow(_fftPos, _fftLength));
            _fftBuffer[_fftPos].Y = 0; // This is always zero with audio.
            _fftPos++;

            if (_fftPos < _fftLength)
                return;

            _fftPos = 0;
            FastFourierTransform.FFT(true, _m, _fftBuffer);
            FftCalculated(this, _fftArgs);
        }
    }
}