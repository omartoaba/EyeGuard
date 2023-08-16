using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Core
{
    public class MonitorInfo
    {
        public uint MinValue { get; set; }
        public uint MaxValue { get; set; }
        public IntPtr Handle { get; set; }
        public uint CurrentValue { get; set; }
        public string MonitorName { get; set; }
        public uint SupportedColorTemperatures { get; set; }
        public uint MonitorCapabilities { get; set; }
        public bool CanChangeBrightness { get; set; }
        public bool CanChangeContrast { get; set; }
    }
}
