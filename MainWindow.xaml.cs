using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CpuDispatcherOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private DispatcherBack _dispatcherBack;
        private DispatcherFore _dispatcherFore;

        private List<TaskItem> _listOfTasks = new List<TaskItem>();

        public MainWindow()
        {
            _dispatcherBack = new DispatcherBack();
            _dispatcherFore = new DispatcherFore();
            InitializeComponent();
            listView.ItemsSource = _listOfTasks;
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            //Clear();
            //SetSystemSpecs();
            //SetButtonStartVisibility(false);
            //listView.Items.Refresh();
            //textBoxTick.IsEnabled = false;
        }

        private void Clear()
        {
            _dispatcherBack = new DispatcherBack();
            _dispatcherFore = new DispatcherFore();
            ClearInfo();
        }

        private void ClearInfo()
        {
            labelTdone.Content = "0";
            labelTwait.Content = "0";
            labelTicks.Content = "0";
            labelSidle.Content = "0";
            textBoxAvWait.Text = "0";
        }

        private void textBoxTick_TextChanged(object sender, TextChangedEventArgs e)
        {
            _dispatcherBack.Tick = Convert.ToInt16(textBoxTick.Text);
            _dispatcherFore.Tick = Convert.ToInt16(textBoxTick.Text);
        }

        private void textBoxWeightFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
            _dispatcherBack.WeightFrom = Convert.ToInt16(textBoxWeightBack.Text);
            _dispatcherBack.WeightTo = Convert.ToInt16(textBoxWeightBack.Text);
        }

        private void textBoxWeightTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            _dispatcherFore.WeightFrom = Convert.ToInt16(textBoxWeightFore.Text);
            _dispatcherFore.WeightTo = Convert.ToInt16(textBoxWeightFore.Text);
        }

        private void textBoxFreqFrom_TextChanged(object sender, TextChangedEventArgs e)
        {
            _dispatcherBack.FreqFrom = Convert.ToInt16(textBoxFreqBack.Text);
            _dispatcherBack.FreqTo = Convert.ToInt16(textBoxFreqBack.Text);
        }

        private void textBoxFreqTo_TextChanged(object sender, TextChangedEventArgs e)
        {
            _dispatcherFore.FreqFrom = Convert.ToInt16(textBoxFreqFore.Text);
            _dispatcherFore.FreqTo = Convert.ToInt16(textBoxFreqFore.Text);
        }
        

        private void FIllInfo()
        {
            labelTdone.Content = _dispatcherBack.ListOfTasks.FindAll(task => task.State == "done").Count;
            labelTwait.Content = _dispatcherBack.ListOfTasks.FindAll(task => task.State == "wait").Count;
            labelTicks.Content = _dispatcherBack.CurrentTick;
            labelSidle.Content = _dispatcherBack.SystemWaitsGenTime;
            textBoxAvWait.Text = ((double)_dispatcherBack.ListOfTasks.Sum(task => task.Wait) /
                _dispatcherBack.ListOfTasks.Count).ToString(CultureInfo.InvariantCulture);
        }

        private void buttonTick_Click(object sender, RoutedEventArgs e)
        {
            _dispatcherBack.MakeOneTick();
            FIllInfo();

            listView.ItemsSource = null;
            listView.ItemsSource = _dispatcherBack.ListOfTasks;
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
