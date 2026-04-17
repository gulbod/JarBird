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
    /// Логика взаимодействия для AddOrdersPage.xaml
    /// </summary>
    public partial class AddOrdersPage : Page
    {
        public Orders CurrentOrder { get; set; }
        public AddOrdersPage()
        {
            InitializeComponent();
            ComboBoxIDUser.ItemsSource = Core.Context.Users.ToList();
            ComboBoxIDStatus.ItemsSource = Core.Context.Statuses.ToList();
            ComboBoxIDProduct.ItemsSource = Core.Context.Products.ToList();

            DatePickerOrderDate.SelectedDate = DateTime.Now;
            TextBoxOrderTime.Text = DateTime.Now.ToString();
            DatePickerDeliveryDate.SelectedDate = DateTime.Now.AddDays(2);
            TextBoxDeliveryTime.Text = DateTime.Now.ToString();

            TextBoxIDOrders.Visibility = Visibility.Collapsed;
            IDOrdersTextBlock.Visibility = Visibility.Collapsed;

            CurrentOrder = new Orders();
            Core.Context.Orders.Add(CurrentOrder);
        }

        public AddOrdersPage(Orders order)
        {
            InitializeComponent();
            CurrentOrder = order;
            DataContext = CurrentOrder;
            ComboBoxIDUser.ItemsSource = Core.Context.Users.ToList();
            ComboBoxIDStatus.ItemsSource = Core.Context.Statuses.ToList();
            ComboBoxIDProduct.ItemsSource = Core.Context.Products.ToList();
            LoadData();
        }
        public void LoadData() 
        {
            TextBoxIDOrders.Text = CurrentOrder.IDOrders.ToString();
            TextBoxDeliveryAddress.Text = CurrentOrder.DeliveryAddress.ToString();
            ComboBoxIDUser.SelectedIndex = Convert.ToInt32(CurrentOrder.IDUser) + 1;
            ComboBoxIDStatus.SelectedIndex = Convert.ToInt32(CurrentOrder.IDStatus) + 1;
            DatePickerOrderDate.SelectedDate = CurrentOrder.OrderDate;
            TextBoxOrderTime.Text = CurrentOrder.OrderDate.ToString();

            DatePickerDeliveryDate.SelectedDate = CurrentOrder.DeliveryDate;
            TextBoxDeliveryTime.Text = CurrentOrder.DeliveryDate.ToString();

            ComboBoxIDProduct.SelectedIndex = Convert.ToInt32(CurrentOrder.IDProduct) + 1;
            TextBoxQuantity.Text = CurrentOrder.Quantity.ToString();
            TextBoxPriceInOrder.Text = CurrentOrder.PriceInOrder.ToString();
            TextBoxLineTotal.Text = CurrentOrder.LineTotal.ToString();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            var MessageBoxResult = MessageBox.Show("Вы точно хотите удалить товар?", "Удалить", MessageBoxButton.YesNo);
            if (MessageBoxResult == MessageBoxResult.Yes)
            {
                Core.Context.Orders.Remove(CurrentOrder);
                Core.Context.SaveChanges();
                NavigationService.Navigate(new OrdersPage());
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxDeliveryAddress.Text) ||
                ComboBoxIDUser.SelectedIndex == -1 ||
                ComboBoxIDStatus.SelectedIndex == -1 ||
                DatePickerOrderDate.SelectedDate == null ||
                string.IsNullOrEmpty(TextBoxOrderTime.Text) ||
                DatePickerDeliveryDate.SelectedDate == null ||
                string.IsNullOrEmpty(TextBoxDeliveryTime.Text) ||
                ComboBoxIDProduct.SelectedIndex == -1 ||
                string.IsNullOrEmpty(TextBoxQuantity.Text))
            {
                MessageBox.Show("Заполните все данные");
                return;
            }
            if (!ValidationData())
            {
                return;
            }
            try
            {
                CurrentOrder.DeliveryAddress = TextBoxDeliveryAddress.Text;
                CurrentOrder.Users = ComboBoxIDUser.SelectedItem as Users;
                CurrentOrder.Statuses = ComboBoxIDStatus.SelectedItem as Statuses;

                if (DatePickerOrderDate.SelectedDate.HasValue)
                {
                    TimeSpan time;
                    if (TimeSpan.TryParse(TextBoxOrderTime.Text, out time))
                    {
                        CurrentOrder.OrderDate = DatePickerOrderDate.SelectedDate.Value.Date + time;
                    }
                }

                if (DatePickerDeliveryDate.SelectedDate.HasValue)
                {
                    TimeSpan time;
                    if (TimeSpan.TryParse(TextBoxDeliveryTime.Text, out time))
                    {
                        CurrentOrder.DeliveryDate = DatePickerDeliveryDate.SelectedDate.Value.Date + time;
                    }
                }

                CurrentOrder.Products= ComboBoxIDProduct.SelectedItem as Products;
                CurrentOrder.Quantity = Convert.ToInt32(TextBoxQuantity.Text);
                CurrentOrder.PriceInOrder = (ComboBoxIDProduct.SelectedItem as Products).Price;
                CurrentOrder.LineTotal = (ComboBoxIDProduct.SelectedItem as Products).Price * Convert.ToInt32(TextBoxQuantity.Text);

                Core.Context.SaveChanges();
                MessageBox.Show("Данные успешно сохранены");
                NavigationService.Navigate(new OrdersPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private bool ValidationData()
        {
            if (!int.TryParse(TextBoxQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Количество должно быть положительным числом");
                return false;
            }

            if (!TimeSpan.TryParse(TextBoxOrderTime.Text, out TimeSpan orderTime))
            {
               
            }

            if (!TimeSpan.TryParse(TextBoxDeliveryTime.Text, out TimeSpan deliveryTime))
            {
                
            }

            if (DatePickerOrderDate.SelectedDate.HasValue && DatePickerDeliveryDate.SelectedDate.HasValue)
            {
                DateTime orderDateTime = DatePickerOrderDate.SelectedDate.Value.Date + orderTime;
                DateTime deliveryDateTime = DatePickerDeliveryDate.SelectedDate.Value.Date + deliveryTime;

                if (deliveryDateTime < orderDateTime)
                {
                    MessageBox.Show("Дата доставки не может быть раньше даты заказа");
                    return false;
                }
            }
            return true;
        }
    }
}
