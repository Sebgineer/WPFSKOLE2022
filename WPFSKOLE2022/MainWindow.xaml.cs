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

namespace WPFSKOLE2022
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Timer timer = new Timer();
        public MainWindow()
        {
            InitializeComponent();
            timer.CountdownChanges += CountDownChange;
        }

        private void CountDownChange(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                CountdownLabel.Content = timer.TimeLeft();
            });
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            timer.SetTimer((int)Seconds.Value, (int)Minutes.Value);
            timer.StartTimer();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.StopTimer();
        }
    }
}
