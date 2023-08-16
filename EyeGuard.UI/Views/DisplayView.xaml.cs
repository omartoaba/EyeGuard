using EyeGuard.Application;
using EyeGuard.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
        public DisplayView()
        {
            InitializeComponent();
            DataContext = App.AppHost.Services.GetService<DisplayViewModel>();
        }
    }
}
