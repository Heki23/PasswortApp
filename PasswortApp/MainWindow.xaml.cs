using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswortApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();

          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string loginname = loginEingebeneName.Text;
            string loginpasswort = loginEingebenePasswort.Password;

            //if (loginname == "test" && loginpasswort == "1234") 
            //{
            ChangeUItoOptionWindow();
                //this.Hide();
                //PasswortFolderUeberschicht passwortFolderUeberschicht = new PasswortFolderUeberschicht();
                //passwortFolderUeberschicht.Show();
                //}
                //else
                //{
                //    //Hier manche ich Eingebefeld leer 
                //    loginEingebeneName.Text = null;
                //    loginEingebenePasswort.Password = null;
                //                      //Beschreibung                  //Title                        
                //    MessageBox.Show("Bitte Name und Passwort prüfen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                //}

        }
        public void ChangeUItoOptionWindow()
        {
            //loginEingebeneName.Visibility = Visibility.Hidden;
            //loginEingebenePasswort.Visibility = Visibility.Hidden;
            //AnmeldenLabel.Visibility = Visibility.Hidden;
            //AnmeldenBtn.Visibility = Visibility.Hidden;
            //loginWhiteRectangle.Visibility = Visibility.Hidden;
            loginWindow.Visibility = Visibility.Hidden;


            //OptionwindowRectangle.Visibility = Visibility.Visible;
            //SpielerButton.Visibility = Visibility.Visible;
            //ArbeitButton.Visibility = Visibility.Visible;
            //PrivateButton.Visibility = Visibility.Visible;
            OptionWindow.Visibility = Visibility.Visible;



        }
        private void SpielerButton_Click(object sender, RoutedEventArgs e)
        {
            //neuPasswordSaverWindow.currentWindow = "SpielerPSaverWindow";
            SpielerPSaverWindow spielerPSaverWindow = new SpielerPSaverWindow();
            spielerPSaverWindow.Show();
        }

        private void ArbeitButton_Click_1(object sender, RoutedEventArgs e)
        {
            //neuPasswordSaverWindow.currentWindow = "ArbeitPSaverWindow";
            ArbeitPSaverWindow arbeitPSaverWindow = new ArbeitPSaverWindow();
            arbeitPSaverWindow.Show();
        }

        private void PrivateButton_Click(object sender, RoutedEventArgs e)
        {
            PrivatePSaverWindow privatePSaverWindow = new PrivatePSaverWindow();
            privatePSaverWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loginEingebeneName.Focus(); // Fokus auf das TextBox setzen
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Down)
            {
                // Das Passwortfeld fokussieren
                loginEingebenePasswort.Focus();
                string login = loginEingebenePasswort.Password;
                //if(loginEingebenePasswort != null)
                //{
                //    AnmeldenBtn.Focus();
                //}
                
            }
        }
     
    }
}
