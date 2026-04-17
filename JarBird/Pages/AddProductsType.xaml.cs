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

namespace JarBird.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddProductsType.xaml
    /// </summary>
    public partial class AddProductsType : Page
    {
        public Products CurrentProduct { get; set; }
        public AddProductsType()
        {
            InitializeComponent();

            IDProductTextBlock2.Visibility = Visibility.Collapsed;
            IDProductTextBlock.Visibility = Visibility.Collapsed;
            CurrentProduct = new Products();
            DataContext = CurrentProduct;
            DelButton.Visibility = Visibility.Collapsed;
            IDProductTypeComboBox.ItemsSource = Core.Context.ProductTypes.ToList();
            Core.Context.Products.Add(CurrentProduct);
        }
        public AddProductsType(Products Product)
        {
            InitializeComponent();
            CurrentProduct = Product;
            DataContext = CurrentProduct;
            IDProductTypeComboBox.ItemsSource = Core.Context.ProductTypes.ToList();
            LoadProduct();
        }

        private void LoadProduct() 
        {
            IDProductTextBlock.Text = Convert.ToString(CurrentProduct.IDProduct);
            ProductNameTextBox.Text = Convert.ToString(CurrentProduct.ProductName);
            IDProductTypeComboBox.SelectedIndex = Convert.ToInt32(CurrentProduct.IDProductType) + 1;
            DescriptionTextBox.Text = Convert.ToString(CurrentProduct.Description);
            CompositionTextBox.Text = Convert.ToString(CurrentProduct.Composition);
            PriceTextBox.Text = Convert.ToString(CurrentProduct.Price);
            DiscountProcentTextBox.Text = Convert.ToString(CurrentProduct.DiscountProcent);
            IsDiscountActiveTextBox.Text = Convert.ToString(CurrentProduct.IsDiscountActive);
            MinStockTextBox.Text = Convert.ToString(CurrentProduct.MinStock);
            QuantityInStockTextBox.Text = Convert.ToString(CurrentProduct.QuantityInStock);
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var MessageBoxResult = MessageBox.Show("Вы точно хотите удалить товар?", "Удалить", MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                Core.Context.Products.Remove(CurrentProduct);
                Core.Context.SaveChanges();
                NavigationService.Navigate(new ProductsPage());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
               string.IsNullOrWhiteSpace(DiscountProcentTextBox.Text) ||
               string.IsNullOrWhiteSpace(IsDiscountActiveTextBox.Text) ||
               string.IsNullOrWhiteSpace(MinStockTextBox.Text) ||
               string.IsNullOrWhiteSpace(QuantityInStockTextBox.Text) ||
               IDProductTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все данные");
                return;
            }
            if (ValidationData())
            {
                return;
            }
            CurrentProduct.ProductName = ProductNameTextBox.Text;
            CurrentProduct.Description = DescriptionTextBox.Text;
            CurrentProduct.Composition = CompositionTextBox.Text;

            CurrentProduct.Price = Convert.ToDouble(PriceTextBox.Text.Replace('.', ','));
            CurrentProduct.DiscountProcent = Convert.ToInt32(DiscountProcentTextBox.Text);
            CurrentProduct.IsDiscountActive = Convert.ToBoolean(IsDiscountActiveTextBox.Text);
            CurrentProduct.MinStock = Convert.ToInt32(MinStockTextBox.Text);
            CurrentProduct.QuantityInStock = Convert.ToInt32(QuantityInStockTextBox.Text);
            CurrentProduct.ProductTypes = IDProductTypeComboBox.SelectedItem as ProductTypes;

            Core.Context.SaveChanges();
            MessageBox.Show("Данные успешно сохранены");
            NavigationService.Navigate(new ProductsPage());
        }

        private bool ValidationData()
        {
            string errorMessage = "";

            string priceText = PriceTextBox.Text.Replace('.', ',');
            if (double.TryParse(priceText, out double price))
            {
                if (price < 0)
                    errorMessage += "Цена не может быть меньше нуля\n";
            }
            else
            {
                errorMessage += "Цена должна быть числом\n";
            }
            if (string.IsNullOrWhiteSpace(DiscountProcentTextBox.Text))
            {
                errorMessage += "Процент скидки не может быть пустым\n";
            }
            else
            {
                if (!int.TryParse(DiscountProcentTextBox.Text, out int discount) || discount < 0 || discount > 100)
                    errorMessage += "Процент скидки должен быть числом от 0 до 100\n";
            }

            if (string.IsNullOrWhiteSpace(IsDiscountActiveTextBox.Text))
            {
                errorMessage += "Поле 'Скидка активна' не может быть пустым\n";
            }
            else
            {
                if (!bool.TryParse(IsDiscountActiveTextBox.Text, out _))
                    errorMessage += "Поле 'Скидка активна' должно быть True или False\n";
            }

            if (string.IsNullOrWhiteSpace(MinStockTextBox.Text))
            {
                errorMessage += "Минимальный запас не может быть пустым\n";
            }
            else
            {
                if (!int.TryParse(MinStockTextBox.Text, out int minStock) || minStock < 0)
                    errorMessage += "Минимальный запас должен быть положительным числом\n";
            }

            if (string.IsNullOrWhiteSpace(QuantityInStockTextBox.Text))
            {
                errorMessage += "Количество на складе не может быть пустым\n";
            }
            else
            {
                if (!int.TryParse(QuantityInStockTextBox.Text, out int quantity) || quantity < 0)
                    errorMessage += "Количество на складе должно быть положительным числом\n";
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show("Обнаружены следующие ошибки:\n\n" + errorMessage,
                                "Ошибка валидации",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return true; 
            }

            return false; 
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage());
        }
    }
}
