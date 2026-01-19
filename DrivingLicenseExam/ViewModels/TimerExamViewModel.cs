using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DrivingLicenseExam.ViewModels
{
    class TimerExamViewModel : INotifyPropertyChanged
    {

        // Change TimeDisplay property type from int to string
        private string _timeDisplay;
        private DispatcherTimer _timer;
        private TimeSpan _remainingTime;
        public string TimeDisplay
        {
            get => _timeDisplay;
            set
            {
                if (_timeDisplay != value)
                {
                    _timeDisplay = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TimerExamViewModel(int totalTimeInSeconds)
        {
            _remainingTime = TimeSpan.FromSeconds(totalTimeInSeconds);
            UpdateDisplay();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_remainingTime.TotalSeconds <= 0)
            {
                _timer.Stop();
                TimeDisplay = "00:00";
                return;
            }

            _remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            TimeDisplay = _remainingTime.ToString(@"mm\:ss");
        }




    }
}
