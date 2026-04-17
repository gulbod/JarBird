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
    /// Логика взаимодействия для AddKorzinaPage.xaml
    /// </summary>
    public partial class AddKorzinaPage : Page
    {
        public Products CurrentProduct { get; set; }

        Korzina CurrentOrder = new Korzina();
        public AddKorzinaPage(Products Product)
        {
            InitializeComponent();
            CurrentProduct = Product;     
            DataContext = CurrentOrder;
            Core.Context.Korzina.Add(CurrentOrder);
        }

        private void AddKorzinaButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(QuantityTextBox.Text))
            {
                MessageBox.Show("Введите количество продукта");
                return;
            }
            CurrentOrder.IDUser = Core.AuthUser.IDUser;
            CurrentOrder.IDProduct = CurrentProduct.IDProduct;
            CurrentOrder.Quantity = Convert.ToInt32(QuantityTextBox.Text);
            CurrentOrder.PriceInOrder = Convert.ToDouble(CurrentProduct.Price);
            CurrentOrder.LineTotal = Convert.ToInt32(QuantityTextBox.Text) * CurrentProduct.Price;
            Core.Context.SaveChanges();
            MessageBox.Show("Продукт добавлен в корзину");
            NavigationService.Navigate(new ProductsPage());
        }
    }
}
