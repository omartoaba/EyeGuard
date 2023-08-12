using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static EyeGuard.Application.BrightnessHelper;

namespace EyeGuard.Application;

internal class ScreenWarmthHelper
{

    public IReadOnlyCollection<MonitorInfo> Monitors { get; set; }
    public IList<MonitorCapibilities> MonitorsCapibilities { get; set; }

    public ScreenWarmthHelper()
    {
        UpdateMonitors();
    }
    public void SetColorTemperature(int temp)
    {
       
    }
    public int GetColorTemperature()
    {
        return -1;
    }
    private void UpdateMonitors()
    {
        DisposeMonitors(this.MonitorsCapibilities);

        MonitorsCapibilities = new List<MonitorCapibilities>();
        NativeAPI.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData) =>
        {
            uint physicalMonitorsCount = 0;
            if (!NativeAPI.GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, ref physicalMonitorsCount))
            {
                // Cannot get monitor count
                return true;
            }

            var physicalMonitors = new PHYSICAL_MONITOR[physicalMonitorsCount];
            if (!NativeAPI.GetPhysicalMonitorsFromHMONITOR(hMonitor, physicalMonitorsCount, physicalMonitors))
            {
                // Cannot get physical monitor handle
                return true;
            }

            foreach (PHYSICAL_MONITOR physicalMonitor in physicalMonitors)
            {
                uint pdwMonitorCapabilities = 0, pdwSupportedColorTemperatures = 0;
                if (!NativeAPI.GetMonitorCapabilities(physicalMonitor.hPhysicalMonitor, out pdwMonitorCapabilities, out pdwSupportedColorTemperatures))
                {
                    NativeAPI.DestroyPhysicalMonitor(physicalMonitor.hPhysicalMonitor);
                    continue;
                }

                var info = new MonitorCapibilities(physicalMonitor.hPhysicalMonitor, pdwMonitorCapabilities, pdwSupportedColorTemperatures);
                MonitorsCapibilities.Add(info);
            }

            return true;
        }, IntPtr.Zero);
    }

    public void Dispose()
    {
        DisposeMonitors(MonitorsCapibilities);
        GC.SuppressFinalize(this);
    }

    private static void DisposeMonitors(IEnumerable<MonitorCapibilities> monitors)
    {
        if (monitors?.Any() == true)
        {
            PHYSICAL_MONITOR[] monitorArray = monitors.Select(m => new PHYSICAL_MONITOR { hPhysicalMonitor = m.Handle }).ToArray();
            DestroyPhysicalMonitors((uint)monitorArray.Length, monitorArray);
        }
    }
    
}
