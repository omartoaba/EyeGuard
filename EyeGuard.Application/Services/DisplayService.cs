using EyeGuard.Application;
using EyeGuard.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Application;

public class DisplayService : IDisplayService,IDisposable
{
    MonitorHelper _monitorHelper;
    IEnumerable<MonitorInfo> _monitors;

    public IEnumerable<MonitorInfo> Monitors => _monitors;

    public DisplayService()
    {
        _monitorHelper = new();
        _monitors = _monitorHelper.GetAvailibleMonitors();
    }
    public bool SetBrightness(int brightnessValue, MonitorInfo monitorInfo)
    {
        //To ensure that the monitor is still available is the caller requested a refresh 
        var monitor = Monitors.FirstOrDefault(m => m.Handle == monitorInfo.Handle);
        if(monitor is null ||!monitor.CanChangeBrightness)
            return false;
        return SetBrightnessInternal((uint)brightnessValue, monitorInfo);
    }

    private bool SetBrightnessInternal(uint brightnessValue, MonitorInfo monitorInfo)
    {
        uint realNewValue = (monitorInfo.MaxValue - monitorInfo.MinValue) * brightnessValue / 100 + monitorInfo.MinValue;
        if (NativeAPI.SetMonitorBrightness(monitorInfo.Handle, realNewValue))
        {
            monitorInfo.CurrentValue = realNewValue;
            return true;
        }
        return false;
    }

    public void SetContrast(int contrast, MonitorInfo monitorInfo)
    {
  
    }

    public int GetBrightness(MonitorInfo monitorInfo)
    {
        var monitor = Monitors.FirstOrDefault(m => m.Handle == monitorInfo.Handle);
        if (monitor is null || !monitor.CanChangeBrightness)
            return -1;
        return (int)monitor.CurrentValue;
    }

    public int GetContrast(MonitorInfo monitorInfo)
    {
       
    }

    public void Dispose()
    {
        _monitorHelper?.Dispose();
    }
    public void RefreshMonitors()
    {
        _monitorHelper.DisposeMonitors();
        _monitors = _monitorHelper.GetAvailibleMonitors();
    }
}


