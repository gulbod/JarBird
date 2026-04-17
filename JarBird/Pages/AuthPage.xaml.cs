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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            { 
                MessageBox.Show("Введите логин");
                return;
            }
            else if(string.IsNullOrWhiteSpace(PassPasswordBox.Password))
            {
                MessageBox.Show("Введите пароль");
                return;
            }
            var User = Core.Context.Users.FirstOrDefault(u =>
                u.Login == LoginTextBox.Text && u.Password == PassPasswordBox.Password);

            if (User == null)
            {
                MessageBox.Show("пользователь не найден");
                return;
            }
            Core.AuthUser = User;
            NavigationService.Navigate(new ProductsPage());

        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductsPage());
        }
    }
}
