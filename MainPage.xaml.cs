using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using static DXGI.DXGITools;

namespace WinUI3_SwapChainPanel_DWriteCore
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        //public SwapChainPanel Scp1 => scp1;
    }
}