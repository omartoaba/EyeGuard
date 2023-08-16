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
    BrightnessHelper _brightnessController;
    ScreenWarmthHelper _screenWarmthHelper;

    public DisplayService()
    {
        _brightnessController = new();
        _screenWarmthHelper = new();
    }
    public bool SetBrightness(int brightness, MonitorInfo monitorInfo)
    {
       return _brightnessController.Set((uint)brightness,monitorInfo);
    }

    public void SetContrast(int contrast, MonitorInfo monitorInfo)
    {
        _screenWarmthHelper.SetColorTemperature(contrast,monitorInfo);
    }

    public int GetBrightness(MonitorInfo monitorInfo)
    {
        return _brightnessController.Get(monitorInfo);
    }

    public int GetContrast(MonitorInfo monitorInfo)
    {
       return _screenWarmthHelper.GetColorTemperature(monitorInfo);
    }

    public void Dispose()
    {
        _brightnessController?.Dispose();
        _screenWarmthHelper?.Dispose();
    }

    public IEnumerable<MonitorInfo> GetMonitors()
    {
       return _brightnessController.Monitors;
    }
    public void RefreshMonitors()
    {
       
    }
}


