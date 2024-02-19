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
                CheckAndCreateDatabase(databasePath);
                MessageBox.Show("Die Datenbank wurde nicht gefunden. Eine Registrierung ist erforderlich.", "Registrierung erforderlich", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            Mouse.OverrideCursor = null;
        }
        #region Benutzerdatenbank erstellen und prüfen
        private void CreateUserDatabaseAndTable(string databasePath)
        {
            // SQL-Abfrage zum Überprüfen, ob die Datenbank existiert und gültig ist
            string checkDatabaseQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = 'UserLoginDB'";

            // SQL-Abfrage zum Löschen der Datenbank
            string dropDatabaseQuery = "DROP DATABASE IF EXISTS UserLoginDB";

            // SQL-Abfrage zum Erstellen der Tabelle UserTabelle
            string createUserTableQuery = "CREATE TABLE UserTabelle (UserName VARCHAR(50) PRIMARY KEY, Password VARCHAR(50))";

            // SQL-Abfrage zum Erstellen der Datenbank UserLoginDB
            string createDatabaseQuery = $"CREATE DATABASE UserLoginDB ON PRIMARY (NAME = UserLoginDB_Data, FILENAME = '{databasePath}')";

            try
            {
                using (SqlConnection masterConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True"))
                {
                    masterConnection.Open();

                    // Überprüfen, ob die Datenbank bereits existiert
                    using (SqlCommand checkDbCommand = new SqlCommand(checkDatabaseQuery, masterConnection))
                    {
                        int dbExists = (int)checkDbCommand.ExecuteScalar();
                        if (dbExists > 0)
                        {
                            // Prüfen, ob die Datenbank physisch vorhanden ist
                            bool isValidDatabase = CheckDatabaseValidity(databasePath);

                            if (!isValidDatabase)
                            {
                                // Löschen der vorhandenen, aber ungültigen Datenbank
                                using (SqlCommand dropDbCommand = new SqlCommand(dropDatabaseQuery, masterConnection))
                                {
                                    dropDbCommand.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Die Datenbank ist bereits vorhanden und gültig, nichts zu tun
                                return;
                            }
                        }

                        // Erstellen der Datenbank
                        using (SqlCommand createDbCommand = new SqlCommand(createDatabaseQuery, masterConnection))
                        {
                            createDbCommand.ExecuteNonQuery();
                        }
                    }
                }

                // Öffnen der Verbindung zur neu erstellten Datenbank UserLoginDB
                using (SqlConnection userDbConnection = new SqlConnection($"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;AttachDbFilename={databasePath}"))
                {
                    userDbConnection.Open();

                    // Erstellen der Tabelle UserTabelle
                    using (SqlCommand createUserTableCommand = new SqlCommand(createUserTableQuery, userDbConnection))
                    {
                        createUserTableCommand.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Die Datenbank wurde erfolgreich erstellt. Bitte registrieren Sie sich.", "Datenbank erstellt", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Versuchen Sie es erneut!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Methode zur Überprüfung, ob die Datenbank physisch vorhanden ist
        private bool CheckDatabaseValidity(string databasePath)
        {
            return File.Exists(databasePath);
        }
        private void CheckAndCreateDatabase(string databasePath)
        {
            // SQL-Abfrage zum Überprüfen, ob die Datenbank existiert und gültig ist
            string checkDatabaseQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = 'UserLoginDB'";
            string physicalCheckQuery = $"SELECT COUNT(*) FROM sys.master_files WHERE name = 'UserLoginDB'";

            try
            {
                using (SqlConnection masterConnection = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True"))
                {
                    masterConnection.Open();

                    using (SqlCommand checkDbCommand = new SqlCommand(checkDatabaseQuery, masterConnection))
                    {
                        int dbExists = (int)checkDbCommand.ExecuteScalar();

                        if (dbExists == 0)
                        {
                            // Datenbank existiert nicht, also erstelle sie
                            CreateUserDatabaseAndTable(databasePath);
                        }
                        else
                        {
                            // Datenbank existiert, überprüfe, ob sie physisch vorhanden ist
                            using (SqlCommand physicalCheckCommand = new SqlCommand(physicalCheckQuery, masterConnection))
                            {
                                int physicalExists = (int)physicalCheckCommand.ExecuteScalar();

                                if (physicalExists == 0)
                                {
                                    // Datenbank ist physisch nicht vorhanden, also erstelle sie erneut
                                    CreateUserDatabaseAndTable(databasePath);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ein Fehler ist beim Überprüfen und Erstellen der Datenbank aufgetreten: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

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
                    try {
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
                    catch
                    {
                        // Rufe die Methode auf, um die Datenbank zu überprüfen und zu erstellen
                        CheckAndCreateDatabase(databasePath);
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Down)
            {
                // Das Passwortfeld fokussieren
                if (!string.IsNullOrEmpty(loginEingebenePasswort.Password))
                {
                    AnmeldenBtn.Focus();
                }
                else
                {
                    loginEingebenePasswort.Focus();
                }

            }
        }

        #region Placeholders
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
        #endregion

        #region Placeholders setting
        private void benutzerNamePlaceholder_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.IBeam;
            loginEingebeneName.Focus();
        }

        private void benutzerNamePlaceholder_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void passwortNamePlaceholder_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.IBeam;
            loginEingebenePasswort.Focus();
        }

        private void passwortNamePlaceholder_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        #endregion
    }
}