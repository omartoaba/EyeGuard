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
    public bool SetBrightness(int brightness)
    {
       return _brightnessController.Set((uint)brightness);
    }

    public void SetContrast(int contrast)
    {
        _screenWarmthHelper.SetColorTemperature(contrast);
    }

    public int GetBrightness()
    {
        return _brightnessController.Get();
    }

    public int GetContrast()
    {
       return _screenWarmthHelper.GetColorTemperature();
    }

    public void Dispose()
    {
        _brightnessController?.Dispose();
    }
}


