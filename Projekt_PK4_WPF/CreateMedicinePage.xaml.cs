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
    /// Interaction logic for CreateMedicinePage.xaml
    /// </summary>
    public partial class CreateMedicinePage : Page
    {
        private Database database;
        private int index;

        private Medicine newMedicine;
        private ReplacementsCreator newReplacements = new ReplacementsCreator();
        private Medicine selectedReplacement;

        private List<string> levelOfFundingList = new List<string>(Enum.GetNames(typeof(LevelOfFunding)));//lista, bo stala liczba elementow
        private int MeidcineRM_LevelStart;


        public CreateMedicinePage()
        {
            this.InitializeComponent();

            Bind();
        }

        public CreateMedicinePage(NavigationParameters navigationParameters)
        {
            database = navigationParameters.database;
            index = navigationParameters.index;

            if (index >= 0)
            {
                newMedicine = database[index].Copy();
            }
            else
            {
                newMedicine = new Medicine();
            }

            MeidcineRM_LevelStart = (int)newMedicine.RM_Level;

            this.InitializeComponent();

            Bind();
        }

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);


        //    var lastPage = Frame.BackStack.LastOrDefault();                     //zapogieganie stackowaniu tej samej strony po odswiezeniu
        //    if (lastPage.SourcePageType.ToString().Contains("CreateMedicinePage") == true)
        //    {
        //        Frame.BackStack.Remove(lastPage);
        //    }

        //    NavigationParameters navigationParameters = e.Parameter as NavigationParameters;
        //    database = navigationParameters.database;
        //    index = navigationParameters.index;

        //    if (index >= 0)
        //    {
        //        newMedicine = database[index].Copy();
        //    }
        //    else
        //    {
        //        newMedicine = new Medicine();
        //    }

        //    MeidcineRM_LevelStart = (int)newMedicine.RM_Level;
        //}


        private void ImageButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            //var lastPage = Frame.BackStack.LastOrDefault();
            //if (lastPage.SourcePageType.ToString().Contains("DatabasePage") == true)
            //{
            //    Frame.Navigate(typeof(DatabasePage), database);
            //}
            //if (lastPage.SourcePageType.ToString().Contains("MedicinePage") == true)         //szuka czeci, a nie porownuje, moze powodowac bledy
            //{
            //    NavigationParameters navigationParameters = new NavigationParameters(database, index);
            //    Frame.Navigate(typeof(MedicinePage), navigationParameters);
            //}
        }

        private void ImageButtonDeleteMed_Click(object sender, RoutedEventArgs e)
        {
            if (index >= 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Czy na pewno chcesz usunąc wybrany lek?", newMedicine.Name, MessageBoxButton.YesNo);
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
            else
            {
                this.NavigationService.Navigate(new DatabasePage(database));
            }
        }

        private void ImageButtonSaveMed_Click(object sender, RoutedEventArgs e)//ustawione AllowFocusOnInteraction="True"
        {
            if (newMedicine.Name != "")
            {
                if (index >= 0)
                {
                    database.EditMedicine(newMedicine, index, newReplacements);
                }
                else
                {
                    database.AddMedicine(newMedicine, newReplacements);
                    index = database.medBase.Count() - 1;//getindex() oraz count w database
                }

                NavigationParameters navigationParameters = new NavigationParameters(database, index);
                this.NavigationService.Navigate(new MedicinePage(navigationParameters));
            }
            else
            {
                MessageBox.Show("Nazwa leku nie może pozostać pusta!", "Uwaga!");
            }
        }

        private void ImageButtonRestoreMed_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.RemoveBackEntry();
            NavigationParameters navigationParameters = new NavigationParameters(database, index);
            this.NavigationService.Navigate(new CreateMedicinePage(navigationParameters));
        }


        private void ComboBoxMedicineRM_LevelChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newMedicine.RM_Level = (LevelOfFunding)ComboBoxMedicineRM_LevelChoice.SelectedIndex;

            if (newMedicine.RM_Reimbursed)
            {
                StackPanelRM_FundingLimit.Visibility = Visibility.Visible;
                StackPanelPriceAfterDiscount.Visibility = Visibility.Visible;
            }
            else
            {
                StackPanelRM_FundingLimit.Visibility = Visibility.Collapsed;
                StackPanelPriceAfterDiscount.Visibility = Visibility.Collapsed;
            }
        }






        //private void AutoSuggestBoxReplacements_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{
        //    if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        //    {
        //        AutoSuggestBoxReplacements.ItemsSource = database.medBase.Where(med => med.Name.IndexOf(sender.Text.ToString()) >= 0).ToList();
        //    }
        //}

        //private void AutoSuggestBoxReplacements_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        //{
        //    var selectedItem = args.SelectedItem as Medicine;
        //    sender.Text = selectedItem.Name;

        //    selectedReplacement = selectedItem;
        //}

        private void ButtonReplacementAdd_Click(object sender, RoutedEventArgs e)
        {
            if (selectedReplacement != null)
            {
                if (newMedicine.replacements.IndexOf(selectedReplacement) < 0)
                {
                    if (index < 0)
                    {
                        newMedicine.replacements.Add(selectedReplacement);
                        newReplacements.medToAdd.Add(selectedReplacement);
                    }
                    else
                    {
                        if (selectedReplacement != database[index])
                        {
                            newMedicine.replacements.Add(selectedReplacement);
                            newReplacements.medToAdd.Add(selectedReplacement);
                        }
                    }
                }
            }
        }

        private void ButtonReplacementDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewReplacements.SelectedItem is Medicine med)
            {
                newMedicine.replacements.Remove(med);

                if (newReplacements.medToAdd.IndexOf(med) < 0)
                {
                    newReplacements.medToDelete.Add(med);
                }
                else
                {
                    newReplacements.medToAdd.Remove(med);
                }
            }
        }



        private void Bind()
        {
            BindingConverterDouble bindingConverterDouble = new BindingConverterDouble();
            BindingConverterInt32 bindingConverterInt32 = new BindingConverterInt32();
            BindingConverterPrice bindingConverterPrice = new BindingConverterPrice();

            Binding binding = new Binding() { Source = database, Path = new PropertyPath("Name") };
            TextBlockDatabaseName.SetBinding(TextBlock.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("Name"), Mode = BindingMode.TwoWay };
            TextBoxMedicineName.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("Type"), Mode = BindingMode.TwoWay };
            TextBoxMedicineType.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("AgeRestrictions"), Mode = BindingMode.TwoWay, Converter = bindingConverterInt32 };
            TextBoxMedicineAgeRestrictions.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("Intended"), Mode = BindingMode.TwoWay };
            TextBoxMedicineIntended.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("Composition"), Mode = BindingMode.TwoWay };
            TextBoxMedicineComposition.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("Comments"), Mode = BindingMode.TwoWay };
            TextBoxMedicineComments.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("Price"), Mode = BindingMode.TwoWay, Converter = bindingConverterDouble };
            TextBoxMedicinePrice.SetBinding(TextBox.TextProperty, binding);

            ComboBoxMedicineRM_LevelChoice.ItemsSource = levelOfFundingList;
            ComboBoxMedicineRM_LevelChoice.SelectedIndex = MeidcineRM_LevelStart;

            ListViewReplacements.ItemsSource = newMedicine.replacements;

            if (newMedicine.RM_Reimbursed) 
            {
                StackPanelRM_FundingLimit.Visibility = Visibility.Visible;
                StackPanelPriceAfterDiscount.Visibility = Visibility.Visible;
            }

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("RM_FundingLimit"), Mode = BindingMode.TwoWay, Converter = bindingConverterDouble };
            TextBoxMedicineRM_FundingLimit.SetBinding(TextBox.TextProperty, binding);

            binding = new Binding() { Source = newMedicine, Path = new PropertyPath("PriceAfterDiscount"), Mode = BindingMode.OneWay, Converter = bindingConverterPrice };
            TextBlockPriceAfterDiscount.SetBinding(TextBlock.TextProperty, binding);


            ComboBoxReplacements.ItemsSource = database.medBase;
        }


        private void TextBoxReplacements_LostFocus(object sender, RoutedEventArgs e)
        {
            var query = database.medBase.Where(med => med.Name.IndexOf(TextBoxReplacements.Text) >= 0);

            ComboBoxReplacements.ItemsSource = query.ToList();
        }

        private void ComboBoxReplacements_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBoxReplacements.SelectedItem is Medicine med)
            {
                selectedReplacement = database.medBase[database.medBase.IndexOf(med)];
            }
        }
    }
}

