using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfKaraokePlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AxAXVLC.AxVLCPlugin2 _vlcPlayer;
        public MainWindow()
        {
            InitializeComponent();
            var _vlcPlayer = new VlcControl();
            vlcHost.Child = _vlcPlayer;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            image.Source = new BitmapImage(new Uri(@"C:\Users\l-bre\Desktop\ares2.png", UriKind.Absolute));

        }
    }
}
