using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Application;

public record MonitorCapibilities(IntPtr Handle, uint PdwMonitorCapabilities, uint PdwSupportedColorTemperatures);
