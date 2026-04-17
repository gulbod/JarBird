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
    public partial class KorzinaPage : Page
    {
        public KorzinaPage()
        {
            InitializeComponent();
            LoadMyOrders();
        }

        /// <summary>
        /// Загружает список товаров в корзине для текущего авторизованного пользователя
        /// </summary>
        public void LoadMyOrders()
        {
            var filteredOrders = Core.Context.Korzina.ToList();
            filteredOrders = filteredOrders.Where(p => p.IDUser == Core.AuthUser.IDUser).ToList();
            KorzinaListBox.ItemsSource = filteredOrders;
        }

        private void KorzinaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(DeliveryAddressTextBox.Text))
            {
                MessageBox.Show("Укажите адрес доставки");
                return;
            }
            if (KorzinaListBox.SelectedItem is Korzina SelectedOrder)
            {
                var messageBoxResult = MessageBox.Show("Нажмите 'Да' чтобы оформить заказ\nНажмите 'Нет' чтобы удалить продукт из корзины\nНажмите 'Отмена' чтобы вернуться",
                                      "Действие с корзиной",
                                      MessageBoxButton.YesNoCancel);

                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    // Оформление заказа
                    var NewOrder = new Orders
                    {
                        DeliveryAddress = DeliveryAddressTextBox.Text,
                        IDUser = Core.AuthUser.IDUser,
                        IDStatus = 1,
                        OrderDate = DateTime.Now,
                        DeliveryDate = DateTime.Now.AddDays(2),
                        IDProduct = SelectedOrder.IDProduct,
                        Quantity = SelectedOrder.Quantity,
                        PriceInOrder = SelectedOrder.PriceInOrder,
                        LineTotal = SelectedOrder.LineTotal
                    };
                    Core.Context.Orders.Add(NewOrder);
                    Core.Context.Korzina.Remove(SelectedOrder);
                    Core.Context.SaveChanges();
                    MessageBox.Show("Заказ успешно создан");
                    LoadMyOrders();
                }
                else if (messageBoxResult == MessageBoxResult.No)
                {
                    // Удаление из корзины
                    var confirmDelete = MessageBox.Show("Вы уверены, что хотите удалить продукт из корзины?",
                                                        "Подтверждение удаления",
                                                        MessageBoxButton.YesNo);
                    if (confirmDelete == MessageBoxResult.Yes)
                    {
                        Core.Context.Korzina.Remove(SelectedOrder);
                        Core.Context.SaveChanges();
                        MessageBox.Show("Продукт удален из корзины");
                        LoadMyOrders();
                    }
                }
            }
        }

        private void AllKorzinaButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DeliveryAddressTextBox.Text))
            {
                MessageBox.Show("Укажите адрес доставки");
                return;
            }
            var allKorzinaItems = Core.Context.Korzina
                .Where(p => p.IDUser == Core.AuthUser.IDUser)
                .ToList();

            if (!allKorzinaItems.Any())
            {
                MessageBox.Show("Корзина пуста");
                return;
            }

            var MessageBoxResult = MessageBox.Show("Хотите оформить все заказы в корзине?", "Оформить", MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                foreach (var korzinaItem in allKorzinaItems)
                {
                    var newOrder = new Orders
                    {
                        DeliveryAddress = DeliveryAddressTextBox.Text,
                        IDUser = Core.AuthUser.IDUser,
                        IDStatus = 1,
                        OrderDate = DateTime.Now,
                        DeliveryDate = DateTime.Now.AddDays(2),
                        IDProduct = korzinaItem.IDProduct, 
                        Quantity = korzinaItem.Quantity,   
                        PriceInOrder = korzinaItem.PriceInOrder, 
                        LineTotal = korzinaItem.LineTotal  
                    };

                    Core.Context.Orders.Add(newOrder);
                    Core.Context.Korzina.Remove(korzinaItem);
                }

                Core.Context.SaveChanges();
                MessageBox.Show("Заказы успешно созданы");
                LoadMyOrders();
                DeliveryAddressTextBox.Clear();
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                LoadMyOrders();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage());
        }
    }
}