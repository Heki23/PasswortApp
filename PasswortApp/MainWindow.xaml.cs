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
using System.Data.SqlClient;
using System.IO;
using System.Net.PeerToPeer;


namespace PasswortApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
      static public string username;


        public MainWindow()
        {
            InitializeComponent();

            username = "TextBox23";

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //string connectionString = Properties.Settings.Default.connectionString;

            string username = UsernameTextBox.Text;
            string databaseName = $"{username}_DB"; // Verwenden Sie einen bestimmten Datenbanknamen, z.B. Benutzername_DB

            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Überprüfen, ob die Datenbank bereits existiert
                string checkDatabaseQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";
                using (SqlCommand command = new SqlCommand(checkDatabaseQuery, connection))
                {
                    int databaseExists = (int)command.ExecuteScalar();
                    if (databaseExists == 0)
                    {
                        // Wenn die Datenbank nicht existiert, erstellen
                        string createDatabaseQuery = $"CREATE DATABASE {databaseName}";
                        using (SqlCommand createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection))
                        {
                            createDatabaseCommand.ExecuteNonQuery();
                        }

                        // Verwenden Sie die neu erstellte Datenbank
                        string useDatabaseQuery = $"USE {databaseName}";
                        using (SqlCommand useDatabaseCommand = new SqlCommand(useDatabaseQuery, connection))
                        {
                            useDatabaseCommand.ExecuteNonQuery();
                        }

                        // Hier erstellen Sie die Tabellen in der neu erstellten Datenbank
                        string createArbeitSDBTableQuery = @"CREATE TABLE ArbeitSDB
                                    (
                                        [App] VARCHAR (50) NOT NULL,
                                        [BenutzerName] VARCHAR (50) NOT NULL,
                                        [Password] VARCHAR (50) NOT NULL,
                                        PRIMARY KEY CLUSTERED ([App] ASC)
                                    )";
                        using (SqlCommand createArbeitSDBTableCommand = new SqlCommand(createArbeitSDBTableQuery, connection))
                        {
                            createArbeitSDBTableCommand.ExecuteNonQuery();
                        }

                        string createPrivateSDBTableQuery = @"CREATE TABLE PrivateSDB
                                    (
                                        [App] VARCHAR (50) NOT NULL,
                                        [BenutzerName] VARCHAR (50) NOT NULL,
                                        [Password] VARCHAR (50) NOT NULL,
                                        PRIMARY KEY CLUSTERED ([App] ASC)
                                    )";
                        using (SqlCommand createPrivateSDBTableCommand = new SqlCommand(createPrivateSDBTableQuery, connection))
                        {
                            createPrivateSDBTableCommand.ExecuteNonQuery();
                        }

                        string createSpieleSDBTableQuery = @"CREATE TABLE SpieleSDB
                                    (
                                        [App] VARCHAR (50) NOT NULL,
                                        [BenutzerName] VARCHAR (50) NOT NULL,
                                        [Password] VARCHAR (50) NOT NULL,
                                        PRIMARY KEY CLUSTERED ([App] ASC)
                                    )";
                        using (SqlCommand createSpieleSDBTableCommand = new SqlCommand(createSpieleSDBTableQuery, connection))
                        {
                            createSpieleSDBTableCommand.ExecuteNonQuery();
                        }

                        MessageBox.Show("Datenbank und Tabellen wurden erfolgreich erstellt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Datenbank existiert bereits.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}