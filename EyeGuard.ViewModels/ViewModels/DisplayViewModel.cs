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
        private int _maximumColorTemperature = 6500;
        [ObservableProperty]
        private int _minimumColorTemperature = 0;
        [ObservableProperty]
        private IEnumerable<MonitorInfo> _monitors;
        [ObservableProperty]
        private bool _canChangeBrightness;
        [ObservableProperty]
        private bool _canChangeColorTemperature;
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
                    CanChangeColorTemperature = SelectedMonitor.CanChangeColorTemperature;
                   // _displayService.GetContrast(SelectedMonitor);
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
            CanChangeColorTemperature = false;
            Monitors = _displayService.Monitors;
          
        }
    }
}
