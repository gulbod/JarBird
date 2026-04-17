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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            ListBoxProducts.ItemsSource = Core.Context.Products.ToList();

            var ProductsTypes = Core.Context.ProductTypes.ToList();
            ProductsTypes.Insert(0, new ProductTypes { ProductTypeName = "Все типы продукции" });

            ProductTypeComboBox.ItemsSource = ProductsTypes;
            if (Core.AuthUser == null)
            {
                AddProductButton.Visibility = Visibility.Collapsed;
                KorzinaButton.Visibility = Visibility.Collapsed;
                OrdersButton.Visibility = Visibility.Collapsed;
            }
            else 
            {
                FIOTextBlock.Text = Core.AuthUser.FIO;
                AuthButton.Visibility = Visibility.Collapsed;
                switch (Core.AuthUser.IDRole)
                {
                    case 1:
                        AddProductButton.Visibility = Visibility.Collapsed;
                        OrdersButton.Visibility = Visibility.Collapsed;
                        break;
                    case 2:
                        KorzinaButton.Visibility = Visibility.Collapsed;
                        break;
                    case 3:
                        KorzinaButton.Visibility = Visibility.Collapsed;
                        break;
                }

            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                ListBoxProducts.ItemsSource = Core.Context.Products.ToList();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void PriceSortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void ProductTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        /// <summary>
        /// Выполняет фильтрацию, поиск и сортировку списка продуктов на основе выбранных пользователем параметров
        /// </summary>
        private void Filter()
        {
            try
            {
                var filteredProducts = Core.Context.Products.ToList();

                if (!string.IsNullOrWhiteSpace(SearchTextBox?.Text))
                {
                    filteredProducts = filteredProducts
                        .Where(p => p.ProductName.ToLower().Contains(SearchTextBox.Text.ToLower()))
                    .ToList();
                }
                if (PriceSortComboBox.SelectedIndex == 1)
                {
                    filteredProducts = filteredProducts.OrderBy(p => p.Price).ToList();
                }
                else if (PriceSortComboBox.SelectedIndex == 2)
                {
                    filteredProducts = filteredProducts.OrderByDescending(p => p.Price).ToList();
                }

                if (ProductTypeComboBox != null && ProductTypeComboBox.SelectedIndex != 0)
                {
                    filteredProducts = filteredProducts
                        .Where(p => p.ProductTypes == ProductTypeComboBox.SelectedItem as ProductTypes)
                        .ToList();
                }

                if (ListBoxProducts != null)
                {
                    ListBoxProducts.ItemsSource = filteredProducts;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации данных: {ex.Message}");
            }
        }

        private void ListBoxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Core.AuthUser == null)
            {
                return;
            }
                
            if (ListBoxProducts.SelectedItem is Products SelectedProduct && (Core.AuthUser.IDRole == 3 || Core.AuthUser.IDRole == 2)) 
            {
                NavigationService.Navigate(new AddProductsType(SelectedProduct));
            }
            if(ListBoxProducts.SelectedItem is Products SelectedProducts && Core.AuthUser.IDRole == 1)
            {
                var MessageBoxResult = MessageBox.Show("Хотите ли добавить этот товар в корзину?", "Добавить", MessageBoxButton.YesNo);
                if (MessageBoxResult == MessageBoxResult.Yes)
                {
                    NavigationService.Navigate(new AddKorzinaPage(SelectedProducts));
                }
            }
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            Core.AuthUser = null;
            NavigationService.Navigate(new AuthPage());
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductsType());
        }

        private void KorzinaButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new KorzinaPage());
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Core.AuthUser = null;
            NavigationService.Navigate(new AuthPage());
        }
    }
}
