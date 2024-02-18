using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient; // Für SQL Server Datenbankverbindung
using System.Windows;

namespace PasswortApp
{
    internal class Viewmodel
    {
        private SqlConnection connection;

        public ObservableCollection<Model> ArbeitPasswordDatenList { get; private set; }
        public ObservableCollection<Model> PrivatePasswordDatenList { get; private set; }
        public ObservableCollection<Model> SpielePasswordDatenList { get; private set; }

        public Viewmodel()
        {
            ArbeitPasswordDatenList = new ObservableCollection<Model>();
            PrivatePasswordDatenList = new ObservableCollection<Model>();
            SpielePasswordDatenList = new ObservableCollection<Model>();

            // string connectionString = $"{Properties.Settings.Default.connectionString}";
           
         //   string databasename = "test45_DB";
           
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Initial Catalog={MainWindow.username}_DB";

            // Verbindung zur Datenbank herstellen
            //  string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30;Initial Catalog={MainWindow.username}";
            //  string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Schueler\\source\\repos\\PasswortApp\\PasswortApp\\Database1.mdf;Integrated Security=True";
            connection = new SqlConnection(connectionString);

            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            connection.Open();

            string queryArbeit = "SELECT App, BenutzerName, Password FROM ArbeitSDB";
            LoadDataFromQuery(queryArbeit, ArbeitPasswordDatenList);

            string queryPrivate = "SELECT App, BenutzerName, Password FROM PrivateSDB";
            LoadDataFromQuery(queryPrivate, PrivatePasswordDatenList);

            string querySpiele = "SELECT App, BenutzerName, Password FROM SpieleSDB";
            LoadDataFromQuery(querySpiele, SpielePasswordDatenList);


        }

        private void LoadDataFromQuery(string query, ObservableCollection<Model> collection)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
              
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    collection.Add(new Model
                    {
                        App = reader["App"].ToString(),
                        Benutzername = reader["BenutzerName"].ToString(),
                        Passwort = reader["Password"].ToString()
                    });
                }
                reader.Close();
            }
        }
        public void DeleteSelectedItemsFromDatabase(Model model, string currentWindowName)
        {

            if (currentWindowName == "ArbeitPSaverWindow")
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM ArbeitSDB WHERE App = @App AND BenutzerName = @BenutzerName AND Password = @Password", connection))
                {
                    //connection.Open();
                    command.Parameters.AddWithValue("@App", model.App);
                    command.Parameters.AddWithValue("@BenutzerName", model.Benutzername);
                    command.Parameters.AddWithValue("@Password", model.Passwort);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            if (currentWindowName == "PrivatePSaverWindow")
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM PrivateSDB WHERE App = @App AND BenutzerName = @BenutzerName AND Password = @Password", connection))
                {
                    //connection.Open();
                    command.Parameters.AddWithValue("@App", model.App);
                    command.Parameters.AddWithValue("@BenutzerName", model.Benutzername);
                    command.Parameters.AddWithValue("@Password", model.Passwort);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            if (currentWindowName == "SpielerPSaverWindow")
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM SpieleSDB WHERE App = @App AND BenutzerName = @BenutzerName AND Password = @Password", connection))
                {
                    //connection.Open();
                    command.Parameters.AddWithValue("@App", model.App);
                    command.Parameters.AddWithValue("@BenutzerName", model.Benutzername);
                    command.Parameters.AddWithValue("@Password", model.Passwort);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
