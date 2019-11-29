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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_PK4_WPF
{
    /// <summary>
    /// Interaction logic for DatabaseLogPage.xaml
    /// </summary>
    public partial class DatabaseLogPage : Page
    {
        private Database database;

        public DatabaseLogPage()
        {
            this.InitializeComponent();

            Bind();
        }

        public DatabaseLogPage(Database database_)
        {
            database = database_;

            this.InitializeComponent();

            Bind();
        }



        private void ImageButtonReturnToDatabase_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DatabasePage(database));
        }



        private void Bind()
        {
            Binding binding = new Binding() { Source = database, Path = new PropertyPath("Name") };
            TextBlockDatabaseName.SetBinding(TextBlock.TextProperty, binding);

            ListViewDatabaseLog.ItemsSource = database.log.Log;
        }
    }
}
