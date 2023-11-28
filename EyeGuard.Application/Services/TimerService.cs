using EyeGuard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace EyeGuard.Application.Services
{
    public class TimerService : ITimerService
    {
        private Timer _timer;
        private bool _initialized = false;
        double _interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
        private TimeSpan _initialeTime = TimeSpan.Zero;
        public TimeSpan CountDown { get; set; } = TimeSpan.Zero;

        public void Dispose()
        {
           _timer?.Dispose();
        }

        public void Initialize(TimeSpan timeSpan, double interval)
        {
            _timer = new Timer();
            _initialeTime = timeSpan;
            CountDown = timeSpan;
            _timer.Interval = interval;
            _initialized = true;
        }

        public void ResetTimer(bool autoStart)
        {
            if (!_initialized)
                throw new ArgumentException("Timer was not Initialized please call Initialize before Calling any other Method");
            _timer.Stop();
            _timer.Start();
            CountDown = _initialeTime;
        }

        public void StartTimer()
        {
            if (!_initialized)
                throw new ArgumentException("Timer was not Initialized please call Initialize before Calling any other Method");
            _timer.Start();
        }

        public void StopTimer()
        {
            if (!_initialized)
                throw new ArgumentException("Timer was not Initialized please call Initialize before Calling any other Method");
            _timer.Stop();
        }
    }
}
