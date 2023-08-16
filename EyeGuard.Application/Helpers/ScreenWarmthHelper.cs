using EyeGuard.Core;
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

internal class ScreenWarmthHelper : IDisposable
{
    MonitorHelper _monitorHelper;
    public IEnumerable<MonitorInfo> Monitors { get; set; }

    public ScreenWarmthHelper()
    {
        _monitorHelper = new MonitorHelper();
        Monitors = _monitorHelper.GetAvailibleMonitors().Where(m => m.CanChangeContrast);
    }
    public void SetColorTemperature(int temp, MonitorInfo monitorInfo)
    {
        foreach (var monitor in Monitors)
            NativeAPI.SetMonitorColorTemperature(monitor.Handle, MC_COLOR_TEMPERATURE.MC_COLOR_TEMPERATURE_4000K);
    }
    public int GetColorTemperature(MonitorInfo monitorInfo)
    {
        return -1;
    }
    private void UpdateMonitors()
    {
      
    }

    public void Dispose()
    {
        _monitorHelper.Dispose();
        GC.SuppressFinalize(this);
    }   
}
