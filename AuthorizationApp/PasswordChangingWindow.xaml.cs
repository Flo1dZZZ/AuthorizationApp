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
using System.Data.SQLite;
using Microsoft.Data.Sqlite;

namespace AuthorizationApp
{
    /// <summary>
    /// Interaction logic for PasswordChangingWindow.xaml
    /// </summary>
    public partial class PasswordChangingWindow : Window
    {
        public User authUser = new User();
        AppContext db;
        string oldPasOfUser;
        char[] oldPasOfUserByPos;
        char[] oldPassByPosField;
        int lenghtOfRandomPositions;
        int[] randomPositions;
        char[] symbolsInRandomPositions;

        public PasswordChangingWindow(User currentUser)
        {
            InitializeComponent();
            txtGreeting.Text = txtGreeting.Text + currentUser.Login.ToString();
            authUser = currentUser;
            db = new AppContext();

            oldPasOfUser = authUser.Password;
            oldPasOfUserByPos = oldPasOfUser.ToCharArray();
            oldPassByPosField = passOldPass.Password.ToCharArray();

            Random rnd = new Random();

            lenghtOfRandomPositions = rnd.Next(1, oldPasOfUserByPos.Length); // Генерация количества разрядов
            randomPositions = new int[lenghtOfRandomPositions]; // Массив, хранящий разряды, которые нужно вводить

            for (int i = 0; i < randomPositions.Length; i++)
            {
                randomPositions[i] = rnd.Next(i, lenghtOfRandomPositions);
            }

            for (int i = 0; i < randomPositions.Length; i++) // Вывод разрядов в текстбокс
            {
                txtPassSymbolsChanging.Text += (randomPositions[i]+1).ToString() + " ";
            }

            symbolsInRandomPositions = new char[randomPositions.Length];

        }

        private void buttonChangePass_Click(object sender, RoutedEventArgs e)
        {
           string pass1 = passOldPass.Password.Trim();
           string pass2 = passOldPassCheck.Password.Trim();
           string passNew = passNewPass.Password.Trim();
           
           
           if (passOldPass.Password.ToString() != authUser.Password.ToString())
           {
               passOldPass.ToolTip = "Неверный пароль!";
               passOldPass.BorderBrush = Brushes.Red;
           }
           else
           {
               passOldPass.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
               passOldPass.ToolTip = null;
           }            
           if (passNew.Length < 5)
           {
               passNewPass.ToolTip = "Пароль должен содержать не менее 5 символов";
               passNewPass.BorderBrush = Brushes.Red;
           }
           else
           {
               passNewPass.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
               passNewPass.ToolTip = null;
           }
            if (!(passOldPass.Password.ToString() != authUser.Password.ToString() && passNew.Length < 5))
            {
                Random rnd = new Random();
                string symbolsInRandomPositionsStr;
                for (int i = 0; i < symbolsInRandomPositions.Length; i++) // Запись в массив значений разрядов, которые нужно ввести
                {
                    symbolsInRandomPositions[i] = oldPasOfUser[randomPositions[i]];
                }

                symbolsInRandomPositionsStr = new string(symbolsInRandomPositions);

                if (passOldPassCheck.Password.ToString() == symbolsInRandomPositionsStr)
                {
                    
                    MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите изменить пароль?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        string sqlExpression = $"UPDATE Users SET password = '{passNewPass.Password.ToString()}' WHERE login = '{authUser.Login.ToString()}';";
                        using (var connection = new SqliteConnection("Data Source=AuthorizationAppDB.db"))
                        {
                            connection.Open();
                            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                        }
                        this.Close();
                        MessageBox.Show("Пароль успешно изменен!");
                    }

                }
                else
                {
                    MessageBox.Show("Вы ввели неверные значения разрядов!");
                }
            }


            //if (passOldPass.Password.ToString() != authUser.Password.ToString())
            //{
            //    passOldPass.ToolTip = "Неверный пароль!";
            //    passOldPass.BorderBrush = Brushes.Red;
            //}
            //else
            //{
            //    passOldPass.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
            //    passOldPass.ToolTip = null;
            //}
            //if (pass1 != pass2)
            //{
            //    passOldPassCheck.ToolTip = "Пароли не совпадают!";
            //    passOldPassCheck.BorderBrush = Brushes.Red;
            //}
            //else
            //{
            //    passOldPassCheck.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
            //    passOldPassCheck.ToolTip = null;
            //}
            //if (passNew.Length < 5)
            //{
            //    passNewPass.ToolTip = "Пароль должен содержать не менее 5 символов";
            //    passNewPass.BorderBrush = Brushes.Red;
            //}
            //else
            //{
            //    passNewPass.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x74, 0x74, 0x74));
            //    passNewPass.ToolTip = null;
            //}
            //
            //if (!(passNew.Length < 5 || pass1 != pass2))
            //{
            //    MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите изменить пароль?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            //    if (messageBoxResult == MessageBoxResult.Yes)
            //    {
            //        string sqlExpression = $"UPDATE Users SET password = '{passNew}' WHERE login = '{authUser.Login.ToString()}';";
            //        using (var connection = new SqliteConnection("Data Source=AuthorizationAppDB.db"))
            //        {
            //            connection.Open();
            //            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            //            command.ExecuteNonQuery();
            //        }                    
            //        this.Close();
            //    }
            //}
        }
    }
}
