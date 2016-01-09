using System;
using NAudio.Wave;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Linq;
using HomeDMX.Controller;
using HomeDMX.ViewModels;

namespace HomeDMX
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWaveIn waveIn;
        private static int fftLength = 128; // NAudio fft wants powers of two!

        // There might be a sample aggregator in NAudio somewhere but I made a variation for my needs
        private SampleAggregator sampleAggregator = new SampleAggregator(fftLength);

        IDmxController controller;
        public MainWindow()
        {
            InitializeComponent();

            controller = new UDmxController();
            var isConnected = controller.IsConnected;
            controller.Reset();

            var controlSetViewModel = new ControlSetViewModel();

            controlSetViewModel.DmxControllers.Add(new DmxViewModel(controller, 1));
            controlSetViewModel.DmxControllers.Add(new DmxViewModel(controller, 2));
            controlSetViewModel.DmxControllers.Add(new DmxViewModel(controller, 3));
            controlSetViewModel.DmxControllers.Add(new DmxViewModel(controller, 4));
            controlSetViewModel.DmxControllers.Add(new DmxViewModel(controller, 5));

            DataContext = controlSetViewModel;


            Closed += MainWindow_Closed;
            //sampleAggregator.FftCalculated += new EventHandler<FftEventArgs>(FftCalculated);
            //sampleAggregator.PerformFft = true;

            //// Here you decide what you want to use as the waveIn.
            //// There are many options in NAudio and you can use other streams/files.
            //// Note that the code varies for each different source.
            //waveIn = new WasapiLoopbackCapture();
            //var waveFormat = waveIn.WaveFormat;
            //waveIn.DataAvailable += OnDataAvailable;
            //waveIn.StartRecording();

        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            controller.Reset();
        }

        private int counter;
        private void FftCalculated(object sender, FftEventArgs e)
        {
            var freq = e.Result.Take(e.Result.Length / 2).Skip(1).Select(c => Math.Sqrt(c.X * c.X + c.Y * c.Y)).ToList();
            var max = freq.Max();
            var freq2 = freq.Select(x => x / max).ToList();
            var beat = false;

            //for (int i = 0; i < 10; i++)
            {
                if (freq2[0] > 0.8)
                    beat = true;
            }
            if (beat)
                counter++;

            //TbCounter.Text = String.Format("{0} beats", counter);
        }
        static void ErasePixel(WriteableBitmap writeableBitmap, int x, int y, byte red)
        {
            if (y < 0) y = 0;
            if (y > 199) y = 199;

            byte[] ColorData = { 0, 0, red, 0 }; // B G R

            Int32Rect rect = new Int32Rect(
                    x, y,
                    1,
                    1);

            writeableBitmap.WritePixels(rect, ColorData, 4, 0);
        }


        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            //if (Dispatcher.InvokeRequired)
            //{
            Dispatcher.BeginInvoke(new EventHandler<WaveInEventArgs>(OnDataAvailable1), sender, e);
        }

        private void OnDataAvailable1(object sender, WaveInEventArgs e)
        {
            //}
            //else

            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            int bufferIncrement = waveIn.WaveFormat.BlockAlign;

            for (int index = 0; index < bytesRecorded; index += bufferIncrement)
            {
                float sample32 = BitConverter.ToSingle(buffer, index);
                sampleAggregator.Add(sample32);
            }
        }
    }
}
