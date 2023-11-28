using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Core
{
    public interface ITimerService : IDisposable
    {
        public TimeSpan CountDown { get; set; }
        void Initialize(TimeSpan timeSpan, double interval);
        void StartTimer();
        void StopTimer();
        void ResetTimer(bool autoStart);
    }
}
