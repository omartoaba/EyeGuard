using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Timers;
using Timer = System.Timers.Timer;

namespace EyeGuard.ViewModels
{
    public partial class TimerViewModel : ObservableObject
    {
        private object _lock = new object();
        Timer _timer;
        TimeSpan _restTime = TimeSpan.FromMinutes(1);
        TimeSpan _step = TimeSpan.FromSeconds(1);
        double _remainingTimeStep;
        [ObservableProperty]
        private string _currentCountDown;
        [ObservableProperty]
        private double _remainingTime;
        public TimerViewModel()
        {
            _timer = new Timer();
            _timer.Interval = _step.TotalMilliseconds;
            _timer.Elapsed += OnTimerElapsed;
            RemainingTime = 100;
            _remainingTimeStep = 100 / _restTime.TotalSeconds;
            _timer.Start();
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            if (_restTime.TotalSeconds <= 0)
            {
                _timer.Stop();
                return;
            }
            _restTime = _restTime.Subtract(_step);
            RemainingTime -= _remainingTimeStep;
            CurrentCountDown = _restTime.ToString();
        }
        [RelayCommand]
        private void ResetCounter()
        {
            _timer.Stop();
            _restTime = TimeSpan.FromMinutes(1);
            RemainingTime = 100;
            CurrentCountDown = _restTime.ToString();
            _timer.Start();
        }
    }
}
