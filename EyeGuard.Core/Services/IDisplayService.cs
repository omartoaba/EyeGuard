using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Core
{
    public interface IDisplayService
    {
        IEnumerable<MonitorInfo> GetMonitors();
        int GetBrightness(MonitorInfo monitorInfo);
        int GetContrast(MonitorInfo monitorInfo);
        bool SetBrightness(int brightness,MonitorInfo monitorInfo);
        void SetContrast(int contrast, MonitorInfo monitorInfo);
        void RefreshMonitors();
    }
}
