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


namespace AuthorizationApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        AppContext db;
        public MainWindow()
        {
            InitializeComponent();
            db = new AppContext();
        }

        private void buttonRegistration_Click(object sender, RoutedEventArgs e)
        {
            string login = txtBoxLogin.Text.Trim();
            string pass1 = passBoxFirst.Password.Trim();
            string pass2 = passBoxSecond.Password.Trim();
            string email = txtBoxEmail.Text.Trim().ToLower();

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

            if (!email.Contains("@") || !email.Contains("."))
            {
                txtBoxEmail.ToolTip = "Email введен некорректно";
                txtBoxEmail.BorderBrush = Brushes.Red;
            }
            else
            {
                txtBoxEmail.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
                txtBoxEmail.ToolTip = null;
            }
            if (!((login.Length < 5 || pass1.Length < 5 || pass1 != pass2 || !email.Contains("@") || !email.Contains("."))))
            {
                User user = new User(login, pass1, email);
                db.Users.Add(user);
                db.SaveChanges();

                AuthWindow authWindow = new AuthWindow();
                authWindow.Show();
                this.Close();

                //MessageBox.Show("Регистрация прошла успешно!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    

        private void buttonAccExists_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
        }
    }
}

