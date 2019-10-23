using System;
using System.Collections.Generic;
using System.IO;
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

using Projekt_PK4;

namespace Projekt_PK4_WPF
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    /// 
    public partial class MainPage : Page
    {
        private Database database;

        public MainPage()
        {
            this.InitializeComponent();
        }



        private void ButtonMenuNewBase_Click(object sender, RoutedEventArgs e)
        {
            GridItemHiddenMenu.Visibility = Visibility.Collapsed;
            GridItemNewBase.Visibility = Visibility.Visible;
            GridItemLoadBase.Visibility = Visibility.Collapsed;
        }

        private void ButtonMenuLoadBase_Click(object sender, RoutedEventArgs e)
        {
            GridItemHiddenMenu.Visibility = Visibility.Collapsed;
            GridItemNewBase.Visibility = Visibility.Collapsed;
            GridItemLoadBase.Visibility = Visibility.Visible;
        }


        private void ButtonCreateBase_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxNewBaseName.Text != "")//
            {
                database = new Database(TextBoxNewBaseName.Text, TextBoxNewBaseAccessPath.Text);

                //Frame.Navigate(typeof(DatabasePage), database);
            }
            else
            {
                MessageBox.Show("Nazwa bazy nie może pozostać pusta!", "Uwaga!");
            }
        }

        private void ButtonLoadBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                database = new Database(TextBoxLoadBaseAccessPath.Text);
            }
            catch (System.AggregateException aex)
            {
                foreach (var ex in aex.Flatten().InnerExceptions)
                {
                    if (ex is FileNotFoundException)
                    {
                        MessageBox.Show("Nie można odnaleźć określonego pliku!", "Wczytywanie nie powiodło się");
                        return;
                    }
                }
                MessageBox.Show("Nie można prawidłowo odczytać danych z pliku!", "Wczytywanie nie powiodło się");
                return;
            }

            //Frame.Navigate(typeof(DatabasePage), database);
        }

        private void ButtonMenuExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
