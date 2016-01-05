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
            labelTdoneFore.Content = _dispatcherFore.ListOfTasks.FindAll(task => task.State == "done").Count.ToString();
            labelTwaitFore.Content = _dispatcherFore.ListOfTasks.FindAll(task => task.State != "done").Count.ToString();

            labelTdoneBack.Content = _dispatcherBack.ListOfTasks.FindAll(task => task.State == "done").Count.ToString();
            labelTwaitBack.Content = _dispatcherBack.ListOfTasks.FindAll(task => task.State != "done").Count.ToString();

            labelTicks.Content = (_dispatcherBack.CurrentTick + 1).ToString();
            labelSidle.Content = (_dispatcherBack.SystemWaitsGenTime + _dispatcherFore.SystemWaitsGenTime).ToString();
            textBoxAvWaitFore.Text = 
                ((double)_dispatcherFore.ListOfTasks.FindAll(task => task.State == "done").Sum(task => task.Wait) /
                _dispatcherFore.ListOfTasks.FindAll(task => task.State == "done").Count)
                .ToString(CultureInfo.InvariantCulture);
            textBoxAvWaitBack.Text =
                ((double)_dispatcherBack.ListOfTasks.FindAll(task => task.State == "done").Sum(task => task.Wait) /
                _dispatcherBack.ListOfTasks.FindAll(task => task.State == "done").Count)
                .ToString(CultureInfo.InvariantCulture);
        }

        private void FillListBoxSequence(List<string> lst1, List<string> lst2)
        {
            foreach (var str in lst1)
                listBoxSequence.Items.Add(str);

            listBoxSequence.Items.Add("");

            foreach (var str in lst2)
                listBoxSequence.Items.Add(str);

            listBoxSequence.Items.Add("");
        }

        private void buttonTick_Click(object sender, RoutedEventArgs e)
        {
            _dispatcherFore.MakeOneTick();
            _dispatcherBack.MakeOneTick();

            FillListBoxSequence(_dispatcherFore.ListOfSequence, _dispatcherBack.ListOfSequence);
            FIllInfo();

            //listView.ItemsSource = null;
            _listOfTasks.Clear();
            _listOfTasks.AddRange(_dispatcherFore.ListOfTasks);
            _listOfTasks.AddRange(_dispatcherBack.ListOfTasks);
            _listOfTasks.Sort((x,y) => x.Appear.CompareTo(y.Appear));
            listView.Items.Refresh();
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
