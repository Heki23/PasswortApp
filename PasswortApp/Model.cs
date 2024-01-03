using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswortApp
{
    internal class Model
    {
        private string app;
        private string benutzername;
        private string passwort;


        public string App { get { return app; } }
        public string Benutzername { get { return benutzername; } }
        public string Passwort { get { return passwort; } }


        public Model(string a, string b, string p)
        {
            app= a;
            benutzername = b;
            passwort= p;
        }


    }
}
