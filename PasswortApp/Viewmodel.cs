using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswortApp
{
    internal class Viewmodel
    {


        private List<Model> arbeitPasswordDatenList = new List<Model>();
        private List<Model> privatePasswordDatenList = new List<Model>();
        private List<Model> spielePasswordDatenList = new List<Model>();



        public Viewmodel() 
        {
            arbeitPasswordDatenList.Add(new Model("Email", "Test", "1234"));
            arbeitPasswordDatenList.Add(new Model("Email2", "Tes2222222222222222222222222222222222222t", "12234"));

            privatePasswordDatenList.Add(new Model("private", "ptest", "123"));
        }

        public List<Model> ArbeitPasswordDatenList { get { return arbeitPasswordDatenList; } set { arbeitPasswordDatenList = value; } }
        public List<Model> PrivatePasswordDatenList { get { return privatePasswordDatenList; } set { privatePasswordDatenList = value; } } 
        public List<Model> SpielePasswordDatenList { get {return spielePasswordDatenList; } set { spielePasswordDatenList = value; } }
    }
}
