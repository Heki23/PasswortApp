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
using System.Security.Principal;
using System.Collections.ObjectModel;

namespace PasswortApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static public string username;
        private string databasePath;

        public MainWindow()
        {
            InitializeComponent();
       
        }
        #region login
        private void SetDatabasePath()
        {
            string currentUser = Environment.UserName;
            string homeDrive = System.IO.Path.GetPathRoot(Environment.SystemDirectory);

            databasePath = System.IO.Path.Combine(homeDrive, $"Users\\{currentUser}\\Documents\\UserLoginDB.mdf");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            username = loginEingebeneName.Text.Trim();

            string loginpasswort = loginEingebenePasswort.Password;
          
            string UserLoginDBConnection = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True;Connect Timeout=30";

            try
            {
                using (SqlConnection connection = new SqlConnection(UserLoginDBConnection))
                {
                    connection.Open();

                    string loginpasswordquery = $"SELECT Password FROM UserTabelle WHERE UserName = '{username}'";

                    using (SqlCommand command = new SqlCommand(loginpasswordquery, connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            string DBPassword = reader.GetString(0);

                            if (DBPassword == loginpasswort)
                            {
                                ChangeUItoOptionWindow();
                            }
                            else
                            {
                                MessageBox.Show("Das Passwort ist ungültig");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Der Benutzername wurde nicht gefunden");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 4060) // Fehlercode für nicht gefundene Datenbank
                {
                    MessageBox.Show("Die Datenbank wurde nicht gefunden. Eine Registrierung ist erforderlich.", "Registrierung erforderlich", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Hier Code einfügen, um die Datenbank und die Benutzertabelle zu erstellen
                    CreateUserDatabaseAndTable(databasePath);
                }
                else
                {
                    MessageBox.Show("Ein Fehler beim Verbinden mit der Datenbank ist aufgetreten.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Mouse.OverrideCursor = null;
        }

        private void CreateUserDatabaseAndTable(string databasePath)
        {
            string createUserTableQuery = "CREATE TABLE UserTabelle (UserName VARCHAR(50) PRIMARY KEY, Password VARCHAR(50))";

            string createDatabaseQuery = $"CREATE DATABASE UserLoginDB ON PRIMARY (NAME = UserLoginDB_Data, FILENAME = '{databasePath}')";

            try
            {
                using (SqlConnection masterConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True"))
                {
                    masterConnection.Open();

                    using (SqlCommand createDbCommand = new SqlCommand(createDatabaseQuery, masterConnection))
                    {
                        createDbCommand.ExecuteNonQuery();
                    }

                    using (SqlCommand createUserTableCommand = new SqlCommand(createUserTableQuery, masterConnection))
                    {
                        createUserTableCommand.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Die Datenbank wurde erfolgreich erstellt. Bitte registrieren Sie sich.", "Datenbank erstellt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ein Fehler ist beim Erstellen der Datenbank aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ChangeUItoOptionWindow()
        {
           loginWindow.Visibility = Visibility.Hidden;
           OptionWindow.Visibility = Visibility.Visible;
        }
        #endregion

        #region PWindows erstellen
        private void SpielerButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            SpielerPSaverWindow spielerPSaverWindow = new SpielerPSaverWindow();
            spielerPSaverWindow.Show();        
        }

        private void ArbeitButton_Click_1(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            ArbeitPSaverWindow arbeitPSaverWindow = new ArbeitPSaverWindow();
            arbeitPSaverWindow.Show();
        }

        private void PrivateButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            PrivatePSaverWindow privatePSaverWindow = new PrivatePSaverWindow();
            privatePSaverWindow.Show();
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loginEingebeneName.Focus(); // Fokus auf das TextBox setzen
            SetDatabasePath();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Down)
            {
                // Das Passwortfeld fokussieren
                loginEingebenePasswort.Focus();
                string login = loginEingebenePasswort.Password;

            }
        }

        private void dBErstellen_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            username = loginEingebeneName.Text.Trim();
            loginEingebeneName.Text = null;
            string loginpasswort = loginEingebenePasswort.Password;
            loginEingebenePasswort.Password = null;


            if (!string.IsNullOrEmpty(username))
            {
                if ((!string.IsNullOrEmpty(loginpasswort)))
                {
                    //UserName und Password in DB eintragen
                    string UserLoginDBConnection = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True;Connect Timeout=30";
                    using (SqlConnection connection = new SqlConnection(UserLoginDBConnection))
                    {
                        connection.Open();
                        string query = "INSERT INTO UserTabelle (UserName, Password) VALUES (@Benutzername, @Passwort)";


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Parameter hinzufügen, um SQL-Injection-Angriffe zu verhindern
                            command.Parameters.AddWithValue("@Benutzername", username);
                            command.Parameters.AddWithValue("@Passwort", loginpasswort);
                            // SQL-Befehl ausführen
                            command.ExecuteNonQuery();
                        }
                    }

                    string databaseName = $"{username}_DB";
                    string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

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

                                MessageBox.Show("Datenbank wurde erfolgreich erstellt und ausgewählt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Datenbank existiert bereits.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Bitte Passwort eingeben!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Bitte Benutzername eingeben!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Mouse.OverrideCursor = null;
        }


        private void RegistrierenBtn_Click(object sender, RoutedEventArgs e)
        {
            loginEingebeneName.Text = null;
            loginEingebenePasswort.Password = null;
            AnmeldenLabel.Content = "Registrieren";
            AnmeldenBtn.Visibility = Visibility.Hidden;
            RegistrierenBtn.Visibility = Visibility.Hidden;
            RegistrierenundDBBtn.Visibility = Visibility.Visible;
            ZuruekBtn.Visibility = Visibility.Visible;
          
        }

        private void ZuruekBtn_Click(object sender, RoutedEventArgs e)
        {
            loginEingebeneName.Text = null;
            loginEingebenePasswort.Password = null;
            AnmeldenLabel.Content = "Anmelden";
            AnmeldenBtn.Visibility = Visibility.Visible;
            RegistrierenBtn.Visibility = Visibility.Visible;
            RegistrierenundDBBtn.Visibility = Visibility.Hidden;
            ZuruekBtn.Visibility = Visibility.Hidden;

        }

        private void loginEingebeneName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(loginEingebeneName.Text))
            {
                benutzerNamePlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                benutzerNamePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void loginEingebenePasswort_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(loginEingebenePasswort.Password))
            {
                passwortNamePlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                passwortNamePlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}