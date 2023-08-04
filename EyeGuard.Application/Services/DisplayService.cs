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
    public DisplayService()
    {
        _brightnessController = new();
    }
    public bool SetBrightness(int brightness)
    {
       return _brightnessController.Set((uint)brightness);
    }

    public void SetContrast(int contrast)
    {
        throw new NotImplementedException();
    }

    public int GetBrightness()
    {
        return _brightnessController.Get();
    }

    public int GetContrast()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        _brightnessController?.Dispose();
    }
}


