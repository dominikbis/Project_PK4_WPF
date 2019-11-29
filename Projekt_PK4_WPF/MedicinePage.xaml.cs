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
    /// Interaction logic for MedicinePage.xaml
    /// </summary>
    public partial class MedicinePage : Page
    {
        private Database database;
        private Medicine medicine;
        private int index;

        public MedicinePage()
        {
            this.InitializeComponent();

            Bind();
        }

        public MedicinePage(NavigationParameters navigationParameters)
        {
            database = navigationParameters.database;
            index = navigationParameters.index;

            medicine = database[index];


            this.InitializeComponent();

            Bind();
        }




        private void ImageButtonReturnToDatabase_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DatabasePage(database));
        }

        private void ImageButtonDeleteMed_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Czy na pewno chcesz usunąc wybrany lek?", medicine.Name, MessageBoxButton.YesNo);
            switch (messageBoxResult)
            {
                case MessageBoxResult.Yes:
                    database.DeleteMedicine(index);
                    this.NavigationService.Navigate(new DatabasePage(database));
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void ImageButtonEditMed_Click(object sender, RoutedEventArgs e)
        {
            NavigationParameters navigationParameters = new NavigationParameters(database, index);
            this.NavigationService.Navigate(new CreateMedicinePage(navigationParameters));
        }

        private void ButtonDisplayReplacement_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewReplacements.SelectedItem is Medicine med)
            {
                index = database.medBase.IndexOf(med);

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                this.NavigationService.Navigate(new MedicinePage(navigationParameters));
            }
        }



        private void Bind()
        {
            BindingConverterDouble bindingConverterDouble = new BindingConverterDouble();
            BindingConverterInt32 bindingConverterInt32 = new BindingConverterInt32();
            BindingConverterPrice bindingConverterPrice = new BindingConverterPrice();

            Binding binding = new Binding() { Source = database, Path = new PropertyPath("Name") };
            TextBlockDatabaseName.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("Name"), Mode = BindingMode.OneWay };
            TextBlockMedicineName.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("Type"), Mode = BindingMode.OneWay };
            TextBlockMedicineType.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("AgeRestrictions"), Mode = BindingMode.OneWay };
            TextBlockMedicineAgeRestrictions.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("Intended"), Mode = BindingMode.OneWay };
            TextBlockMedicineIntended.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("Composition"), Mode = BindingMode.OneWay };
            TextBlockMedicineComposition.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("Comments"), Mode = BindingMode.OneWay };
            TextBlockMedicineComments.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("Price"), Mode = BindingMode.OneWay, Converter = bindingConverterPrice };
            TextBlockMedicinePrice.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("RM_Level"), Mode = BindingMode.OneWay };
            TextBlockMedicineRM_Level.SetBinding(TextBlock.TextProperty, binding);


            if (medicine.RM_Reimbursed) 
            {
                StackPanelMedicineRM_FundingLimit.Visibility = Visibility.Visible;
                StackPanelMedicinePriceAfterDiscount.Visibility = Visibility.Visible;
            }

            binding = new Binding() { Source = medicine, Path = new PropertyPath("RM_FundingLimit"), Mode = BindingMode.OneWay, Converter = bindingConverterPrice };
            TextBlockMedicineRM_FundingLimit.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = medicine, Path = new PropertyPath("PriceAfterDiscount"), Mode = BindingMode.OneWay, Converter = bindingConverterPrice };
            TextBlockMedicinePriceAfterDiscount.SetBinding(TextBlock.TextProperty, binding);


            ListViewReplacements.ItemsSource = medicine.replacements;
        }
    }
}
