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
                this.Hide();
                PasswortFolderUeberschicht passwortFolderUeberschicht = new PasswortFolderUeberschicht();
                passwortFolderUeberschicht.Show();
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

     
    }
}
