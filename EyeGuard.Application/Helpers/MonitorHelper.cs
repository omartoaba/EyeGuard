using EyeGuard.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Application
{
    internal class MonitorHelper : IDisposable
    {
        PHYSICAL_MONITOR[] _monitors;
        internal IEnumerable<MonitorInfo> GetAvailibleMonitors()
        {
            DisposeMonitors();
            var availibleMonitors = new List<MonitorInfo>();
            NativeAPI.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData) =>
            {
                uint physicalMonitorsCount = 0;
                if (!NativeAPI.GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, ref physicalMonitorsCount))
                    return true;
                _monitors = new PHYSICAL_MONITOR[physicalMonitorsCount];
                if (!NativeAPI.GetPhysicalMonitorsFromHMONITOR(hMonitor, physicalMonitorsCount, _monitors))
                    return true;
                foreach (PHYSICAL_MONITOR physicalMonitor in _monitors)
                {
                    uint minValue = 0, currentValue = 0, maxValue = 0;
                    var monitorInfo = new MonitorInfo() { Handle = physicalMonitor.hPhysicalMonitor, MonitorName = physicalMonitor.szPhysicalMonitorDescription };
                    if (NativeAPI.GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, ref minValue, ref currentValue, ref maxValue))
                    {
                        monitorInfo.CanChangeBrightness = true;
                        monitorInfo.MinValue = minValue;
                        monitorInfo.MaxValue = maxValue;
                        monitorInfo.CurrentValue = currentValue;
                    }
                    uint PdwMonitorCapabilities = 0, PdwSupportedColorTemperatures = 0;          
                    if (NativeAPI.GetMonitorCapabilities(physicalMonitor.hPhysicalMonitor,ref  PdwMonitorCapabilities, ref PdwSupportedColorTemperatures))
                    {
                        monitorInfo.CanChangeColorTemperature = true;
                        monitorInfo.SupportedColorTemperatures = PdwSupportedColorTemperatures;

                        monitorInfo.MonitorCapabilities = PdwMonitorCapabilities;
                    } 
                    if (!monitorInfo.CanChangeBrightness && !monitorInfo.CanChangeColorTemperature)
                        NativeAPI.DestroyPhysicalMonitor(physicalMonitor.hPhysicalMonitor);
                    else availibleMonitors.Add(monitorInfo);
                }

                return true;
            }, IntPtr.Zero);
            return availibleMonitors;
        }
        public void Dispose()
        {
            DisposeMonitors();
            GC.SuppressFinalize(this);
        }
        internal void DisposeMonitors()
        {
            if (_monitors is not null && _monitors.Length > 0)
            {
                NativeAPI.DestroyPhysicalMonitors((uint)_monitors.Length, _monitors);
                _monitors = null;
            }
        }
    }
}
