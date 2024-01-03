using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
using System.Windows.Threading;

namespace PasswortApp
{
    /// <summary>
    /// Interaktionslogik für WindowSpielerPasswort.xaml
    /// </summary>
    public partial class SpielerPSaverWindow : Window
    {
        private NeuPasswordSaverWindow neuPasswordSaverWindow = new NeuPasswordSaverWindow();
        private Viewmodel viewmodel = new Viewmodel();

        public SpielerPSaverWindow()
        {
            InitializeComponent();
            benutzerdatenlistbox.ItemsSource = viewmodel.SpielePasswordDatenList;
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += saveNewPassword;
            timer.Start();
        }

        private void Neue_Click(object sender, RoutedEventArgs e)
        {
          // NeuPasswordSaverWindow neuPasswordSaverWindow = new NeuPasswordSaverWindow();
            neuPasswordSaverWindow.Show();
        }
        private void saveNewPassword(object sender, EventArgs e)
        {

            if (neuPasswordSaverWindow.afterSaveButtonClicken == true)
            {
                MessageBox.Show(neuPasswordSaverWindow.afterSaveButtonClicken.ToString(), "endlich");

                //viewmodel.SpielePasswordDatenList.Add(new Model("asd", "sdsd", "2123"));
                viewmodel.SpielePasswordDatenList.Add(new Model(neuPasswordSaverWindow.neuAppTextBox, neuPasswordSaverWindow.neuAnmeldeName, neuPasswordSaverWindow.neuPasswortTextBox));
                neuPasswordSaverWindow.afterSaveButtonClicken = false;
                benutzerdatenlistbox.Items.Refresh();
            }
    }

    }
}
