using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.Application
{
    public enum MC_COLOR_TEMPERATURE
    {
        MC_COLOR_TEMPERATURE_UNKNOWN,
        MC_COLOR_TEMPERATURE_4000K,
        MC_COLOR_TEMPERATURE_5000K,
        MC_COLOR_TEMPERATURE_6500K,
        MC_COLOR_TEMPERATURE_7500K
    }
    //[StructLayout(LayoutKind.Sequential)]
    //public struct MC_COLOR_TEMPERATURE
    //{
    //    public int D6500K;
    //    public int D5000K;
    //    public int D9300K;
    //    public int CustomColor;
    //}

    [StructLayout(LayoutKind.Sequential)]
    public struct MC_DISPLAY_TECHNOLOGY_TYPE
    {
        public int NotSupported;
        public int CRT;
        public int DFP;
        public int PDEV;
        public int TVOUT;
        public int DONGLE;
        public int Miracast;
        public int SBOX;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MC_SUPPORTED_COLOR_TEMPERATURE
    {
        public int MC_SUPPORTED_COLOR_TEMPERATURE_NONE;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_4000K;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_5000K;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_6500K;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_7500K;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_8200K;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_9300K;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_10000K;
        public int MC_SUPPORTED_COLOR_TEMPERATURE_11500K;
    }
    public enum LPDWORD
    {
        MC_CAPS_BRIGHTNESS,
        MC_CAPS_CONTRAST,
        MC_CAPS_COLOR_TEMPERATURE,
        MC_CAPS_DEGAUSS,
        MC_CAPS_DISPLAY_AREA_POSITION,
        MC_CAPS_DISPLAY_AREA_SIZE,
        MC_CAPS_MONITOR_TECHNOLOGY_TYPE,
        MC_CAPS_NONE,
        MC_CAPS_RED_GREEN_BLUE_DRIVE,
        MC_CAPS_RED_GREEN_BLUE_GAIN,
        MC_CAPS_RESTORE_FACTORY_COLOR_DEFAULTS,
        MC_CAPS_RESTORE_FACTORY_DEFAULTS,
        MC_RESTORE_FACTORY_DEFAULTS_ENABLES_MONITOR_SETTINGS
    }
}
