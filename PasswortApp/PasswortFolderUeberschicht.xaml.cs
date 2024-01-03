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
    /// Interaktionslogik für PasswortFolderUeberschicht.xaml
    /// </summary>
    public partial class PasswortFolderUeberschicht : Window
    {
       

        public PasswortFolderUeberschicht()
        {
            InitializeComponent(); 
        }



        private void SpielerButton_Click(object sender, RoutedEventArgs e)
        {
            SpielerPSaverWindow spielerPSaverWindow = new SpielerPSaverWindow();
            spielerPSaverWindow.Show();
        }

        private void ArbeitButton_Click_1(object sender, RoutedEventArgs e)
        {
          ArbeitPSaverWindow arbeitPSaverWindow = new ArbeitPSaverWindow();
           arbeitPSaverWindow.Show();
        }

        private void PrivateButton_Click(object sender, RoutedEventArgs e)
        {
            PrivatePSaverWindow privatePSaverWindow = new PrivatePSaverWindow();
            privatePSaverWindow.Show();
        }
    }
}
