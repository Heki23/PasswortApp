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
using System.Data.SqlClient; // Für SQL Server Datenbankverbindung
using Microsoft.Identity.Client;

namespace PasswortApp
{
    /// <summary>
    /// Interaktionslogik für NeuPasswordSaverWindow.xaml
    /// </summary>
    /// 

    public partial class NeuPasswordSaverWindow : Window
    {
        public event EventHandler DataSaved;
        public bool afterSaveButtonClicken { get;set;}
        public string neuAppTextBox;
        public string neuAnmeldeName;
        public string neuPasswortTextBox;
        public string currentWindow;


        public NeuPasswordSaverWindow()
        {
            InitializeComponent();
        }

        private void SpeichernBtn_Click(object sender, RoutedEventArgs e)
        {
            // Hier initialisieren Sie die eingegebenen Werte von den TextBoxen
            neuAppTextBox = AppTextBox.Text;
            neuAnmeldeName = AnmeldenameTextBox.Text;
            neuPasswortTextBox = PasswortTextBox.Text;


            // Überprüfen, ob alle TextBoxen ausgefüllt sind
            if (!string.IsNullOrEmpty(neuAppTextBox) && !string.IsNullOrEmpty(neuAnmeldeName) && !string.IsNullOrEmpty(neuPasswortTextBox))
            {
                try
                {
                    // Verbindung zur Datenbank herstellen (Beispielverbindungszeichenfolge)
                    string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Initial Catalog={MainWindow.username}_DB";
                    string query = "";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        Viewmodel viewmodel = new Viewmodel();
                        if (currentWindow == "ArbeitPSaverWindow") 
                        { 
                        // SQL-Befehl zum Einfügen der Daten
                        query = "INSERT INTO ArbeitSDB (App, BenutzerName, Password) VALUES (@App, @Benutzername, @Passwort)";
                        }
                        if (currentWindow == "SpielerPSaverWindow")
                        {
                            query = "INSERT INTO SpieleSDB (App, BenutzerName, Password) VALUES (@App, @Benutzername, @Passwort)";
                        }
                        if (currentWindow == "PrivatePSaverWindow")
                        {
                            query = "INSERT INTO PrivateSDB (App, BenutzerName, Password) VALUES (@App, @Benutzername, @Passwort)";
                        }
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Parameter hinzufügen, um SQL-Injection-Angriffe zu verhindern
                            command.Parameters.AddWithValue("@App", neuAppTextBox);
                            command.Parameters.AddWithValue("@Benutzername", neuAnmeldeName);
                            command.Parameters.AddWithValue("@Passwort", neuPasswortTextBox);
                            // SQL-Befehl ausführen
                            command.ExecuteNonQuery();
                        }
                    }
                    // Nach erfolgreichem Speichern Fenster schließen
                    afterSaveButtonClicken = true;
                    DataSaved?.Invoke(this, EventArgs.Empty);
                    Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Speichern der Daten: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus!");
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Anstatt Window schleißen, nur blenden
            e.Cancel = true;
            Hide();
        }
        #region Placeholders
        private void AppTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AppTextBox.Text))
            {
                appPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                appPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void AnmeldenameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AnmeldenameTextBox.Text))
            {
                namePlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                namePlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void PasswortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PasswortTextBox.Text))
            {
                passwortPlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                passwortPlaceholder.Visibility = Visibility.Visible;
            }
        }
        #endregion
  

        private void appPlaceholder_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.IBeam;
            AppTextBox.Focus();
        }

        private void appPlaceholder_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void passwortPlaceholder_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.IBeam;
            PasswortTextBox.Focus();
        }
        private void passwortPlaceholder_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void namePlaceholder_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.IBeam;
            AnmeldenameTextBox.Focus();
        }

        private void namePlaceholder_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

      
    }
}
