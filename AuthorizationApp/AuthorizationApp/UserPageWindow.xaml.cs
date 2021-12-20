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
using System.Windows.Threading;
using System.Threading;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using System.Data;

namespace AuthorizationApp
{
    /// <summary>
    /// Interaction logic for UserPageWindow.xaml
    /// </summary>
    public partial class UserPageWindow : Window
    {
        public User currentUser = new User();
        AppContext db;
        public UserPageWindow(User authUser)
        {
            InitializeComponent();
            currentUser = authUser;
            db = new AppContext();


            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();

            txtGreeting.Text = txtGreeting.Text + authUser.Login.ToString() + "!";

            string dbConnection = "Data Source=AuthorizationAppDB.db";
            SQLiteConnection connection = new SQLiteConnection(dbConnection);
            try
            {

                connection.Open();
                string query = "select login,email from Users";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataTable dt = new DataTable("Users");
                adapter.Fill(dt);
                dataGridUsers.ItemsSource = dt.DefaultView;
                adapter.Update(dt);

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

            if (authUser.Login.ToString() == "admin" && authUser.Password.ToString() == "admin")
            {
                btnDeleteUser.Visibility = Visibility.Hidden;
                btnDeleteUserAdmin.Visibility = Visibility.Visible;
                btnChangePassword.Visibility = Visibility.Hidden;
                txtBoxId.Visibility = Visibility.Visible;

                try
                {

                    connection.Open();
                    string query = "select id,login,password,email from Users";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.ExecuteNonQuery();

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                    DataTable dt = new DataTable("Users");
                    adapter.Fill(dt);
                    dataGridUsers.ItemsSource = dt.DefaultView;
                    adapter.Update(dt);

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            

            //AppContext db = new AppContext();
            //List<User> users = db.Users.ToList();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            LiveTimeLabel.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите удалить пользователя?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string sqlExpression = $"DELETE FROM Users WHERE login='{currentUser.Login}'";
                using (var connection = new SqliteConnection("Data Source=AuthorizationAppDB.db"))
                {
                    connection.Open();
                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
                AuthWindow authWindow = new AuthWindow();
                authWindow.Show();
                this.Close();
            }
        }

        

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            PasswordChangingWindow passwordChangingWindow = new PasswordChangingWindow(currentUser);
            passwordChangingWindow.Show();
        }

        private void btnDeleteUserAdmin_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(txtBoxId.Text, out id))
            {
                if (id != 1)
                {


                    MessageBoxResult messageBoxResult = MessageBox.Show("Вы уверены, что хотите удалить пользователя?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        string sqlExpression = $"DELETE FROM Users WHERE id='{int.Parse(txtBoxId.Text)}'";
                        using (var connection = new SqliteConnection("Data Source=AuthorizationAppDB.db"))
                        {
                            connection.Open();
                            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Нельзя удалить Администратора","", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите верный id","",MessageBoxButton.OK,MessageBoxImage.Error);
            }              
                
            }
        }
    }
