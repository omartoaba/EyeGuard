using CommunityToolkit.Mvvm.ComponentModel;
using EyeGuard.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeGuard.ViewModels
{
    public partial class DisplayViewModel : ObservableObject
    {
        private readonly IDisplayService _displayService;

        [ObservableProperty]
        private int _maximumBrightness = 100;
        [ObservableProperty]
        private int _minimumBrightness = 0;
        [ObservableProperty]
        private IEnumerable<MonitorInfo> _monitors;
        [ObservableProperty]
        private bool _canChangeBrightness;
        [ObservableProperty]
        private bool _canChangeContrast;
        private MonitorInfo _selectedMonitor;

        public MonitorInfo SelectedMonitor
        {
            get { return _selectedMonitor; }
            set
            {
                if(SetProperty(ref _selectedMonitor, value))
                {
                    CurrentBrightness = (int)SelectedMonitor.CurrentValue;
                    CanChangeBrightness = SelectedMonitor.CanChangeBrightness;
                    CanChangeContrast = SelectedMonitor.CanChangeContrast;
                }
            }
        }

        private int _currentBrightness;
        public int CurrentBrightness
        {
            get { return _currentBrightness; }
            set 
            { 
                if(SetProperty(ref _currentBrightness, value))
                {
                    if(SelectedMonitor != null)
                    _displayService.SetBrightness(_currentBrightness,SelectedMonitor);
                }
            }
        }

        public DisplayViewModel(IDisplayService displayService)
        {
            _displayService = displayService;
            CanChangeBrightness = false; 
            CanChangeContrast = false;
            Monitors = _displayService.GetMonitors();
        }


    }
}
