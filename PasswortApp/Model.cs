namespace PasswortApp
{
    internal class Model
    {
        // Machen Sie die Eigenschaften schreibbar (setzbar)
        public string App { get; set; }
        public string Benutzername { get; set; }
        public string Passwort { get; set; }

        // Sie können auch einen parameterlosen Konstruktor hinzufügen, falls erforderlich
        public Model() { }

        // Konstruktor mit Parametern, um Werte beim Erstellen des Objekts festzulegen
        public Model(string app, string benutzername, string passwort)
        {
            App = app;
            Benutzername = benutzername;
            Passwort = passwort;
        }
    }
}
