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

namespace PasswortApp
{
    /// <summary>
    /// Interaktionslogik für NeuPasswordSaverWindow.xaml
    /// </summary>
    /// 

    public partial class NeuPasswordSaverWindow : Window
    {
        public bool afterSaveButtonClicken { get;set;}
        public string neuAppTextBox;
        public string neuAnmeldeName;
        public string neuPasswortTextBox;

        public NeuPasswordSaverWindow()
        {
            InitializeComponent();
         }

        private void SpeichernBtn_Click(object sender, RoutedEventArgs e)
        {
            //Hier installisern ich eingebene werte von TextBox
            neuAppTextBox = AppTextBox.Text.ToString();
            //Hier mache ich TextBox leer
            AppTextBox.Text = null;
            neuAnmeldeName = AnmeldenameTextBox.Text.ToString();
            AnmeldenameTextBox.Text = null;
            neuPasswortTextBox = PasswortTextBox.Text.ToString();
            PasswortTextBox.Text = null;

            afterSaveButtonClicken = true;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Anstatt Window schleißen, nur blenden
            e.Cancel = true;
            Hide();
        }
    }
}
