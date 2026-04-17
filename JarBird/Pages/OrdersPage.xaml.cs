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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        /// <summary>
        /// Инициализирует новый экземпляр страницы заказов
        /// </summary>
        public OrdersPage()
        {
            InitializeComponent();
            OrdersListBox.ItemsSource = Core.Context.Orders.ToList();
            if(Core.AuthUser.IDRole != 3)
            {
                AddOrderButton.Visibility = Visibility.Collapsed;
            }
        }

        private void OrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrdersListBox.SelectedItem is Orders selectedOrders && Core.AuthUser.IDRole == 3)
            {
                NavigationService.Navigate(new AddOrdersPage(selectedOrders));
            }
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddOrdersPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage());
        }
    }
}
