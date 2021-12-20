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
using System.Windows.Shapes;

namespace AuthorizationApp
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    /// 
    public partial class AuthWindow : Window
    {

        public AuthWindow()
        {
            InitializeComponent();
        }

        private void buttonAuth_Click(object sender, RoutedEventArgs e)
        {
            string login = txtBoxLogin.Text.Trim();
            string pass1 = passBoxFirst.Password.Trim();
            string pass2 = passBoxSecond.Password.Trim();

            Style style = this.FindResource("MaterialDesignFloatingHintTextBox") as Style;

            if (login.Length < 5)
            {

                txtBoxLogin.ToolTip = "Логин должен содержать не менее 5 символов";
                txtBoxLogin.BorderBrush = Brushes.Red;
            }
            else
            {
                txtBoxLogin.ToolTip = null;
                txtBoxLogin.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
            }

            if (pass1.Length < 5)
            {
                passBoxFirst.ToolTip = "Пароль должен содержать не менее 5 символов";
                passBoxFirst.BorderBrush = Brushes.Red;
            }
            else
            {
                passBoxFirst.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
                passBoxFirst.ToolTip = null;
            }

            if (pass1 != pass2)
            {
                passBoxSecond.ToolTip = "Пароли не совпадают!";
                passBoxSecond.BorderBrush = Brushes.Red;
            }
            else
            {
                passBoxSecond.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
                passBoxSecond.ToolTip = null;
            }
             
            if (!((login.Length < 5 || pass1.Length < 5 || pass1 != pass2 )))
            {
                User authUser = null;
                using (AppContext db = new AppContext())
                {
                    authUser = db.Users.Where(user => user.Login == login && user.Password == pass1).FirstOrDefault();
                }

                if (authUser != null)
                {
                    UserPageWindow userPageWindow = new UserPageWindow(authUser);
                    userPageWindow.Show();  
                    this.Close();
                    
                }
                else
                {
                    MessageBox.Show("Такого пользователя нет!");
                }

                //MessageBox.Show("Регистрация прошла успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonAccDoesntExist_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
