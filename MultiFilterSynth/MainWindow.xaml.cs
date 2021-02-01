using SyntCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiFilterSynth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SynthPlatform synthPlatform;
        public MainWindow()
        {
            synthPlatform = new SynthPlatform();
            synthPlatform.SetTempo(160);
            InitializeComponent();
            StartAudio();
        }
        private void ChangeDestination(int oscillator, int input)
        {
            //int setBit = 1;
            //setBit <<= input;
            var matrix = synthPlatform.GetDestinationMatrix(oscillator);
            matrix ^= (1 << input);
            synthPlatform.SetDestination(oscillator, matrix);
        }
        
        private void StartAudio()
        {
            synthPlatform.Start();
        }
        private void ChangeOscFreq(int channel, RoutedPropertyChangedEventArgs<double> e)
        {
            var freq = (int)e.NewValue * 10;
            synthPlatform.SetFrequency(channel, freq);
        }
        private void ChangeOscVol(int channel, RoutedPropertyChangedEventArgs<double> e)
        {
            var vol = (float)e.NewValue;
            vol /= 100;
            synthPlatform.SetVolOfOsc(channel, vol);
        }
        private Tuple<int,int> GetNumberOfButton(object sender)
        {
            var button = (Button)sender;
            var destination = int.Parse(button.Name.Substring(9,1));
            var oscillator = int.Parse(button.Name.Substring(7,1));
            oscillator--;
            var tuple = new Tuple<int, int>(oscillator, destination);
            return tuple;
        }

        #region sliders

        private void Slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(0,e);
        }

        private void Slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(1, e);
        }
        private void Slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(2, e);
        }
        private void Slider4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(3, e);
        }
        private void Slider5_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(4, e);
        }
        private void Slider6_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(5, e);
        }
        private void Slider7_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(6, e);
        }
        private void Slider8_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscFreq(7, e);
        }
        
        private void VolSlider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(0, e);
        }

        private void SeqSlider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void SeqSlider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void VolSlider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(1, e);
        }

        private void VolSlider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(2, e);
        }

        private void SeqSlider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Slider4_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void VolSlider4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(3, e);
        }

        private void SeqSlider4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void VolSlider5_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(4, e);
        }

        private void SeqSlider5_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void VolSlider6_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(5, e);
        }

        private void SeqSlider6_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void VolSlider7_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(6, e);
        }

        private void SeqSlider7_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void VolSlider8_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeOscVol(7, e);
        }

        private void SeqSlider8_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
        #endregion

        #region buttons osc 1

        private void DestinationButtons_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Green ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Green;
            var selection = GetNumberOfButton(sender);
            ChangeDestination(selection.Item1, selection.Item2);
        }

        

        private void SequencerButtons_Click(object sender, RoutedEventArgs e)
        {
            //TODO
            //change function of seq stage 1
        }

        #endregion
    }
}
