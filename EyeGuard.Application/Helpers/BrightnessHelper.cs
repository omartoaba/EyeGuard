using EyeGuard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EyeGuard.Application;

/// <summary>
/// this code was taken from <see cref="https://stackoverflow.com/questions/4013622/adjust-screen-brightness-using-c-sharp"/>
/// </summary>
internal class BrightnessHelper : IDisposable
{
    MonitorHelper _monitorHelper;
    public IEnumerable<MonitorInfo> Monitors { get; private set; }

    public BrightnessHelper()
    {
        _monitorHelper = new MonitorHelper();
       Monitors = _monitorHelper.GetAvailibleMonitors().Where(m => m.CanChangeBrightness);
       // UpdateMonitors();
    }
    private MonitorInfo GetAndRefreshMonitor(MonitorInfo monitorInfo)
    {
        _monitorHelper.DisposeMonitors();
        Monitors = _monitorHelper.GetAvailibleMonitors().Where(m => m.CanChangeBrightness);
        var monitor = Monitors.FirstOrDefault(m => m.Handle == monitorInfo.Handle);
        return monitor;
    }
    public bool Set(uint brightness, MonitorInfo monitorInfo)
    {
        //Refreshing monitors in case monitor brightness changed
        var monitor = GetAndRefreshMonitor(monitorInfo);
        if(monitor is null)
            return false;
        return SetInternal(brightness, monitor);
    }

    private bool SetInternal(uint brightness, MonitorInfo monitorInfo)
    {
        uint realNewValue = (monitorInfo.MaxValue - monitorInfo.MinValue) * brightness / 100 + monitorInfo.MinValue;
        if (NativeAPI.SetMonitorBrightness(monitorInfo.Handle, realNewValue))
        {
            monitorInfo.CurrentValue = realNewValue;
            return true;
        }
        return false;
    }

    public int Get(MonitorInfo monitorInfo)
    {
        var target = GetAndRefreshMonitor(monitorInfo);
        return target is null ? -1 : (int)target.CurrentValue;
    }
    public void Dispose()
    {
        _monitorHelper.Dispose();
        GC.SuppressFinalize(this);
    }
}
