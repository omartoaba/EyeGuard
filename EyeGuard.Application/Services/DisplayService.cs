using ColorMine.ColorSpaces;
using ColorMine.ColorSpaces.Comparisons;
using EyeGuard.Application;
using EyeGuard.Core;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static EyeGuard.Application.NativeAPI;

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
        //To ensure that the monitor is still available if the caller requested a refresh 
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
        if (monitor is null)
            return -1;
        return (int)monitor.CurrentValue;
    }

    public unsafe int GetContrast(MonitorInfo monitorInfo)
    {
        //var result = new NativeAPI.RAMP();
        //var handle = Graphics.FromHwnd(monitorInfo.Handle).GetHdc().ToInt32();
        //var success = NativeAPI.GetDeviceGammaRamp(handle, ref result);
        //double[] colors = TemperatureToColors(4000);
        ////var currentRammp = new ushort[256 * 3];
        ////result.Red.CopyTo(currentRammp, 0);
        ////result.Green.CopyTo(currentRammp, 256);
        ////result.Blue.CopyTo(currentRammp, 256 * 2);
        ////ushort* rammps = GenerateGammaRampValues(colors, currentRammp, handle);

        //new ScreenWarmthHelper().SetColorTemperature(1200, monitorInfo);

        //NativeAPI.SetDeviceGammaRamp(handle, ref result);
        return -1;
    }
    public static unsafe bool SetBrightness(short brightness,Int32 hdc)
    {
   

        if (brightness > 255)
            brightness = 255;

        if (brightness < 0)
            brightness = 0;

        short* gArray = stackalloc short[3 * 256];
        short* idx = gArray;

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 256; i++)
            {
                int arrayVal = i * (brightness + 128);

                if (arrayVal > 65535)
                    arrayVal = 65535;

                *idx = (short)arrayVal;
                idx++;
            }
        }

        //For some reason, this always returns false?
        bool retVal = NativeAPI.SetDeviceGammaRamp(hdc,gArray);

        //Memory allocated through stackalloc is automatically free'd
        //by the CLR.

        return retVal;
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
    private double[] TemperatureToColors(float temp)
    {
        double xc,yc;
        if(temp >= 1667.0 && temp <= 4000.0)
        {
            xc = -0.2661239 * (Math.Pow(10.0, 9.0) / Math.Pow(temp, 3.0)) - 0.2343580 * (Math.Pow(10.0, 6.0) / Math.Pow(temp, 2.0)) + 0.8776956 * (Math.Pow(10.0, 3.0) / temp) + 0.179910;
        } else
        {
            xc = (-3.0258469 * (Math.Pow(10.0, 9.0) / Math.Pow(temp, 3.0))) + 2.1070379 * (Math.Pow(10.0, 6.0) / Math.Pow(temp, 2.0)) + 0.2226347 * (Math.Pow(10.0, 3.0) / temp) + 0.240390;
        }
        if(temp >= 1667.0 && temp <= 2222.0)
        {
            yc = -1.1063814 * Math.Pow(xc, 3.0) - 1.34811020 * Math.Pow(xc, 2.0) + 2.18555832 * xc - 0.20219683;
        }else if(temp > 2222.0 && temp <= 4000.0)
        {
            yc = -0.9549476 * Math.Pow(xc, 3.0) - 1.37418593 * Math.Pow(xc, 2.0) + 2.09137015 * xc - 0.16748867;
        }else
        {
            yc = 3.0817580 * Math.Pow(xc, 3.0) - 5.87338670 * Math.Pow(xc, 2.0) + 3.75112997 * xc - 0.37001483;
        }
        IColorSpace white = new Xyz() { X = xc,Y = yc,Z = 1.0};
        IRgb colors = white.ToRgb();
        return new double[] { colors.R / 255.0, colors.G / 255.0, colors.B / 255.0 };
    }
    // 	for c in range(3) :
    //for i, value in enumerate(gamma_table_gen(RAMP_SIZE, white[c], self.gamma[c])) :

    //             ramp[i + 256 * c] = c_ushort(int(USHORT_MAX* value))

 //   def gamma_table_gen(size, white, gamma):
	//"""Generator function to build a gamma ramp of the given size, white point, and gamma."""
	//for i in range(size) :

 //       yield math.pow(float(i)/float(size-1), 1.0/float (gamma)) * float (white)
    private unsafe ushort* GenerateGammaRampValues(double[] whitepoints, ushort[] currentRammp, int handle)
    {
        ushort* newRamp = stackalloc ushort[256*3];
        ushort* idx = newRamp;
        float alpha = (1200 % 100) / 100.0f;
        whitepoints = new double[3] { 1.0000, 0.6636, 0.3583 };
        
        whitepoints = InterpolateColor(alpha,new double[] {1.00000000,  0.30942099,  0.00000000},new double[] { 1.00000000, 0.42322816, 0.00000000 },new double[3]);
        for (int i = 0; i < 3; i++)
        {
          //  var result = GenerateGammaArray(256, whitepoints[i], 1.0);

            for (int j = 0; j < currentRammp.Length; j++)
            {
               *idx = (ushort)(Math.Pow((currentRammp[j] / (65535)) * whitepoints[i] *100,1.0 /2.2) * (65535));
                idx++;  
            }
        }
       // (float)Math.Pow(Y * setting.Brightness * whitePoint[C], 1.0 / setting.Gamma[C]);
        NativeAPI.SetDeviceGammaRamp(handle, newRamp);
        return newRamp;
    }
    static double[] InterpolateColor(float a, double[] c1, double[] c2, double[] c)
    {
        c[0] = (1.0f - a) * c1[0] + a * c2[0];
        c[1] = (1.0f - a) * c1[1] + a * c2[1];
        c[2] = (1.0f - a) * c1[2] + a * c2[2];
        return c;
    }
    private double[] GenerateGammaArray(int size, double whitepoint,double gamma)
    {
        var result = new double[size];  
        for (int i = 0; i < size; i++)
            result[i] = Math.Pow(i / size - 1, 1.0 / gamma) * whitepoint;
        return result;
    }
    class Win32Gamma
    {
        const ushort USHORT_MAX = 65535;
        const int RAMP_SIZE = 256;

        IntPtr dc;
        ushort[] gammaRamp;
        ushort[] oldRamp;
        double[] gamma = { 2.2, 2.2, 2.2 };

        public Win32Gamma()
        {
            dc = User32.GetDC(IntPtr.Zero);
            gammaRamp = new ushort[RAMP_SIZE * 3];
            oldRamp = new ushort[RAMP_SIZE * 3];

            if (!Gdi32.GetDeviceGammaRamp(dc, oldRamp))
            {
                throw new InvalidOperationException("GetDeviceGammaRamp failed.");
            }
        }

        public void SetGamma(double[] newGamma)
        {
            gamma = newGamma;
        }

        public void AdjustWhitePoint(double[] white)
        {
            ushort[] ramp = new ushort[RAMP_SIZE * 3];

            for (int c = 0; c < 3; c++)
            {
                int channelOffset = RAMP_SIZE * c;
                foreach (double value in GammaTableGen(RAMP_SIZE, white[c], gamma[c]))
                {
                    ramp[channelOffset] = (ushort)(USHORT_MAX * value);
                    channelOffset++;
                }
            }

            Gdi32.SetDeviceGammaRamp(dc, ramp);
        }

        public void Restore()
        {
            Gdi32.SetDeviceGammaRamp(dc, oldRamp);
        }

        IEnumerable<double> GammaTableGen(int size, double white, double gamma)
        {
            for (int i = 0; i < size; i++)
            {
                double normalizedValue = (double)i / (size - 1);
                double gammaCorrectedValue = Math.Pow(normalizedValue, 1.0 / gamma) * white;
                yield return gammaCorrectedValue;
            }
        }

        static class User32
        {
            [DllImport("user32.dll")]
            public static extern IntPtr GetDC(IntPtr hWnd);
        }

        static class Gdi32
        {
            [DllImport("gdi32.dll")]
            public static extern bool GetDeviceGammaRamp(IntPtr hDC, ushort[] ramp);

            [DllImport("gdi32.dll")]
            public static extern bool SetDeviceGammaRamp(IntPtr hDC, ushort[] ramp);
        }

        static void Main(string[] args)
        {
            Win32Gamma gammaController = new Win32Gamma();

            // Example usage:
            double[] newGamma = { 2.2, 2.2, 2.2 };
            double[] newWhite = { 1.0, 1.0, 1.0 };

            gammaController.SetGamma(newGamma);
            gammaController.AdjustWhitePoint(newWhite);

            Console.WriteLine("Gamma adjusted.");
            Console.ReadLine();

            gammaController.Restore();
        }
    }
}


