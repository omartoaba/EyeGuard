﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Application;

/// <summary>
/// this code was taken from <see cref="https://stackoverflow.com/questions/4013622/adjust-screen-brightness-using-c-sharp"/>
/// </summary>
internal class BrightnessHelper : IDisposable
{
    #region DllImport
    [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);

    [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

    [DllImport("dxva2.dll", EntryPoint = "GetMonitorBrightness")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetMonitorBrightness(IntPtr handle, ref uint minimumBrightness, ref uint currentBrightness, ref uint maxBrightness);

    [DllImport("dxva2.dll", EntryPoint = "SetMonitorBrightness")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetMonitorBrightness(IntPtr handle, uint newBrightness);

    [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitor")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DestroyPhysicalMonitor(IntPtr hMonitor);

    [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitors")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize, [In] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

    [DllImport("user32.dll")]
    static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);
    delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);
    #endregion

    private IReadOnlyCollection<MonitorInfo> Monitors { get; set; }

    public BrightnessHelper()
    {
        UpdateMonitors();
    }

    #region Get & Set
    public bool Set(uint brightness)
    {
        return Set(brightness, true);
    }

    private bool Set(uint brightness, bool refreshMonitorsIfNeeded)
    {
        bool isSomeFail = false;
        if (Monitors.Count <= 0)
             return false;
        foreach (var monitor in Monitors)
        {
            uint realNewValue = (monitor.MaxValue - monitor.MinValue) * brightness / 100 + monitor.MinValue;
            if (SetMonitorBrightness(monitor.Handle, realNewValue))
            {
                monitor.CurrentValue = realNewValue;
            }
            else if (refreshMonitorsIfNeeded)
            {
                isSomeFail = true;
                break;
            }
        }

        if (refreshMonitorsIfNeeded && (isSomeFail || !Monitors.Any()))
        {
            UpdateMonitors();
            return Set(brightness, false);
        }
        return isSomeFail;
    }

    public int Get()
    {
        if (!Monitors.Any())
        {
            return -1;
        }
        return (int)Monitors.Average(d => d.CurrentValue);
    }
    #endregion

    private void UpdateMonitors()
    {
        DisposeMonitors(this.Monitors);

        var monitors = new List<MonitorInfo>();
        EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData) =>
        {
            uint physicalMonitorsCount = 0;
            if (!GetNumberOfPhysicalMonitorsFromHMONITOR(hMonitor, ref physicalMonitorsCount))
            {
                // Cannot get monitor count
                return true;
            }

            var physicalMonitors = new PHYSICAL_MONITOR[physicalMonitorsCount];
            if (!GetPhysicalMonitorsFromHMONITOR(hMonitor, physicalMonitorsCount, physicalMonitors))
            {
                // Cannot get physical monitor handle
                return true;
            }

            foreach (PHYSICAL_MONITOR physicalMonitor in physicalMonitors)
            {
                uint minValue = 0, currentValue = 0, maxValue = 0;
                if (!GetMonitorBrightness(physicalMonitor.hPhysicalMonitor, ref minValue, ref currentValue, ref maxValue))
                {
                    DestroyPhysicalMonitor(physicalMonitor.hPhysicalMonitor);
                    continue;
                }

                var info = new MonitorInfo
                {
                    Handle = physicalMonitor.hPhysicalMonitor,
                    MinValue = minValue,
                    CurrentValue = currentValue,
                    MaxValue = maxValue,
                    MonitorName = physicalMonitor.szPhysicalMonitorDescription
                };
                monitors.Add(info);
            }

            return true;
        }, IntPtr.Zero);

        this.Monitors = monitors;
    }

    public void Dispose()
    {
        DisposeMonitors(Monitors);
        GC.SuppressFinalize(this);
    }

    private static void DisposeMonitors(IEnumerable<MonitorInfo> monitors)
    {
        if (monitors?.Any() == true)
        {
            PHYSICAL_MONITOR[] monitorArray = monitors.Select(m => new PHYSICAL_MONITOR { hPhysicalMonitor = m.Handle }).ToArray();
            DestroyPhysicalMonitors((uint)monitorArray.Length, monitorArray);
        }
    }

    #region Classes
   
    #endregion
}
