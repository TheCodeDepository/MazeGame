using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MazeGame
{
    public class Time : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public string TimeElapsed { get; set; }

        private DispatcherTimer timer;
        private Stopwatch stopWatch;

        public void Start()
        {
            timer = new DispatcherTimer();
            timer.Tick += dispatcherTimerTick_;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            stopWatch = new Stopwatch();
            stopWatch.Start();
            timer.Start();
        }
        public void Stop()
        {

            stopWatch.Stop();
            timer.Stop();
        }

        private void dispatcherTimerTick_(object sender, EventArgs e)
        {
            string t = new TimeSpan(0, 0, 0, (int)stopWatch.Elapsed.TotalSeconds).ToString();
            TimeElapsed = t; // Format as you wish
            OnPropertyChanged(new PropertyChangedEventArgs("TimeElapsed"));
        }
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}