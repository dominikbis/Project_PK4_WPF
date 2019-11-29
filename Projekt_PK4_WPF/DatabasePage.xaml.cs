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
    /// Interaction logic for DatabasePage.xaml
    /// </summary>
    public partial class DatabasePage : Page
    {
        private Database database;

        private DatabaseSearch databaseSearch;

        private bool TestingMode = true;

        public DatabasePage()
        {
            this.InitializeComponent();
        }

        public DatabasePage(Database database_)
        {
            database = database_;
            databaseSearch = new DatabaseSearch(database.medBase);

            this.InitializeComponent();

            Bind();
        }




        private void ImageButtonReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Czy chcesz zapisać bazę przed powrotem do menu głównego?", "Uwaga",MessageBoxButton.YesNoCancel);
            switch(messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    database.Save();
                    this.NavigationService.Navigate(new MainPage());
                    break;
                case MessageBoxResult.No:
                    this.NavigationService.Navigate(new MainPage());
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void ImageButtonSaveBase_Click(object sender, RoutedEventArgs e)
        {
            //database.Save();
            //database.SaveLog();

            database.Save();
        }


        private void ImageButtonAddMed_Click(object sender, RoutedEventArgs e)
        {
            NavigationParameters navigationParameters = new NavigationParameters(database, -1);
            this.NavigationService.Navigate(new CreateMedicinePage(navigationParameters));
        }

        private void ImageButtonSearchMed_Click(object sender, RoutedEventArgs e)
        {
            if (GridSearchCriteria.Visibility == Visibility.Visible)
            {
                GridSearchCriteria.Visibility = Visibility.Collapsed;
                databaseSearch.ClearCriteria(database.medBase);
            }
            else
            {
                GridSearchCriteria.Visibility = Visibility.Visible;
            }
        }

        private void ImageButtonDeleteMed_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewDatabase.SelectedItem is Medicine med)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Czy na pewno chcesz usunąc wybrany lek?", med.Name,MessageBoxButton.YesNo);
                switch(messageBoxResult)
                {
                    case MessageBoxResult.Yes:
                        database.DeleteMedicine(med);
                        databaseSearch.displayedMedBase.RemoveAt(ListViewDatabase.SelectedIndex);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        private void ImageButtonDisplayMed_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewDatabase.SelectedItem is Medicine med)
            {
                int index = database.medBase.IndexOf(med);

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                this.NavigationService.Navigate(new MedicinePage(navigationParameters));
            }
        }

        private void ImageButtonEditMed_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewDatabase.SelectedItem is Medicine med)
            {
                int index = database.medBase.IndexOf(med);

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                this.NavigationService.Navigate(new CreateMedicinePage(navigationParameters));
            }
        }

        private void ImageButtonDisplayLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DatabaseLogPage(database));
        }



        private void ControlBoxSearchMed_TextChanged(object sender, TextChangedEventArgs e)
        {
            databaseSearch.Search(database.medBase);
        }

        private void CheckBoxSearchMedReimbursed_Click(object sender, RoutedEventArgs e)
        {
            databaseSearch.Search(database.medBase);
        }

        private void ButtonSearchMedClear_Click(object sender, RoutedEventArgs e)
        {
            databaseSearch.ClearCriteria(database.medBase);
        }





















        private void ImageButtonTESTING_R_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                RandomDatabase.GenerateRandomReplacementsInDatabase(database);
            }
        }

        private void ImageButtonTESTING_M_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                RandomDatabase.AddRandomMedicineToDatabase(database);
            }

            databaseSearch.Search(database.medBase);
        }


        private void Bind()
        {
            BindingConverterDouble converterDouble = new BindingConverterDouble();
            BindingConverterInt32 converterInt32 = new BindingConverterInt32();
            BindingConverterPrice converterPrice = new BindingConverterPrice();


            Binding binding = new Binding() { Source = database, Path = new PropertyPath("Name") };
            TextBlockDatabaseName.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = databaseSearch, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Path = new PropertyPath("Name") };
            TextBoxSearchMedName.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = databaseSearch, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Path = new PropertyPath("Type") };
            TextBoxSearchMedType.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = databaseSearch, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Converter = converterDouble, Path = new PropertyPath("MaxPrise") };
            TextBoxSearchMedPrice.SetBinding(TextBox.TextProperty, binding);


            binding = new Binding() { Source = databaseSearch, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Path = new PropertyPath("Intended") };
            TextBoxSearchMedIntended.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = databaseSearch, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Path = new PropertyPath("Composition") };
            TextBoxSearchMedComposition.SetBinding(TextBox.TextProperty, binding);


            binding = new Binding() { Source = databaseSearch, Mode = BindingMode.TwoWay, Path = new PropertyPath("Reimbursed") };
            CheckBoxSearchMedReimbursed.SetBinding(CheckBox.IsCheckedProperty, binding);

            ListViewDatabase.ItemsSource = databaseSearch.displayedMedBase;


            if (TestingMode) 
            {
                SeparatorTestingMode.Visibility = Visibility.Visible;
                ImageButtonTESTING_R.Visibility = Visibility.Visible;
                ImageButtonTESTING_M.Visibility = Visibility.Visible;
            }


        }

    }
}

