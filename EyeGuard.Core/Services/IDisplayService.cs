using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Core
{
    public interface IDisplayService
    {
        public MonitorInfo Monitors { get; set; }
        int GetBrightness();
        int GetContrast();
        bool SetBrightness(int brightness);
        void SetContrast(int contrast);
    }
}
