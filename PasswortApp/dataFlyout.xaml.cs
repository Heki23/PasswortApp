using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasswortApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class dataFlyout : ContentPage
    {
        public ListView ListView;

        public dataFlyout()
        {
            InitializeComponent();

            BindingContext = new dataFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        private class dataFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<dataFlyoutMenuItem> MenuItems { get; set; }

            public dataFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<dataFlyoutMenuItem>(new[]
                {
                    new dataFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new dataFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new dataFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new dataFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new dataFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}