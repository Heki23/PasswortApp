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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PasswortApp
{
    /// <summary>
    /// Interaktionslogik für WindowArbeitPassword.xaml
    /// </summary>
    public partial class ArbeitPSaverWindow : Window
    {
        private Viewmodel viewmodel = new Viewmodel();

        public ArbeitPSaverWindow()
        {
            InitializeComponent();
            benutzerdatenlistbox.ItemsSource = viewmodel.ArbeitPasswordDatenList;
        }

        private void Neue_Click(object sender, RoutedEventArgs e)
        {
            NeuPasswordSaverWindow neuPasswordSaverWindow = new NeuPasswordSaverWindow();
            neuPasswordSaverWindow.Show();
        }
    }
}
