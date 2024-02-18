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
    /// Interaktionslogik für WindowPrivatePassword.xaml
    /// </summary>
    public partial class PrivatePSaverWindow : Window
    {
        NeuPasswordSaverWindow neuPasswordSaverWindow = new NeuPasswordSaverWindow();

        public PrivatePSaverWindow()
        {
            InitializeComponent();
            RefreshListView();
        }

        private void RefreshListView()
        {
            //Hier habe ich deshalb gemacht das die List automatisch aktualsiert ohne große Code
            Viewmodel viewmodel = new Viewmodel();
            benutzerdatenlistbox.ItemsSource = viewmodel.PrivatePasswordDatenList;
        }

        private void Neue_Click(object sender, RoutedEventArgs e)
        {
            // Fenster für das Speichern eines neuen Passworts anzeigen
            neuPasswordSaverWindow.currentWindow = "PrivatePSaverWindow";
            neuPasswordSaverWindow.ShowDialog(); // Verwenden Sie ShowDialog, um auf das Ergebnis zu warten
            if (neuPasswordSaverWindow.afterSaveButtonClicken)
            {
                // Wenn das Speichern erfolgreich war, aktualisieren Sie die ListView
                RefreshListView();
            }
        }

     

        private void BenutzernameCopy_Click(object sender, RoutedEventArgs e)
        {
            //Hier bekommt ich welche Btn, angeklickt wurde
            Button button = (Button)sender;

            // Dynamische Variable zum Speichern des zugehörigen Datenelements
            //weil der Typ des Datenelements zur Kompilierungszeit nicht bekannt ist.
            //Statische Typisierung umgehen
            //Eigenschaften des Objekts zugreifen, indem Namen als strings verwenden, ohne den Typ des Objekts explizit angeben zu müssen
            dynamic dataItem = button.DataContext;
            //Bekommt Daten von data item
            string benutzername = dataItem.Benutzername;
            //wird zu Clipboard Kopiert 
            Clipboard.SetText(benutzername);

        }
        private void PasswordCopy_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            dynamic dataItem = button.DataContext;
            string passwort = dataItem.Passwort;
            Clipboard.SetText(passwort);

        }

        private void Loeschen_Click(object sender, RoutedEventArgs e)
        {
            Viewmodel viewmodel2 = new Viewmodel();
            //Bekommt die ausgewählten Elemente in der ListView
            var selectedItems = benutzerdatenlistbox.SelectedItems.Cast<Model>();

            // Jedes ausgewählte Element durchlaufen
            foreach (var selectedItem in selectedItems)
            {
                //Aus der Liste löschen
                viewmodel2.ArbeitPasswordDatenList.Remove(selectedItem);
                string currentWindowName = "PrivatePSaverWindow";
                //Aus datenbank löschen
                viewmodel2.DeleteSelectedItemsFromDatabase(selectedItem, currentWindowName);
            }
            RefreshListView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                suchePlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                suchePlaceholder.Visibility = Visibility.Visible;
            }
        }

    }

}
