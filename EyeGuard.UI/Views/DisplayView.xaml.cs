using EyeGuard.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EyeGuard.UI.Views
{
    /// <summary>
    /// Interaction logic for DisplayView.xaml
    /// </summary>
    public partial class DisplayView : UserControl
    {
        DisplayService display = new(); 

        public DisplayView()
        {
            InitializeComponent();
            // brightness.Value = display.GetBrightness();
            //  brightness.ValueChanged += Slider_ValueChanged;
            warmth.ValueChanged += Slider_ValueChanged;
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            display.SetContrast((int)e.NewValue);
          //  display.SetBrightness((int)e.NewValue);
        }
    }
}
