using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace FlowerShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для registration_login.xaml
    /// </summary>
    public partial class registration_login : Page
    {
        private string userEmail;
        public registration_login()
        {
            InitializeComponent();
            LoadExistingData();
        }

        public registration_login(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadExistingData();
        }
        private void registration_login_back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new authorization());
        }

        private void registration_login_next(object sender, RoutedEventArgs e)
        {
            string email = LoginTextBox.Text;
            string password1 = PasswordBox1.Password;
            string password2 = PasswordBox2.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(password2))
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Введите корректный логин.");
                return;
            }

            if (password1 != password2)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            if (!IsValidPassword(password1))
            {
                MessageBox.Show("Пароль не соответствует требованиям безопасности.");
                return;
            }

            // Сохранение пользователя в базу данных
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = "INSERT INTO Users (Email, Password) VALUES (@Email, @Password)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password1);
                    cmd.ExecuteNonQuery();

                    // Сохранение email в сессии
                    UserSession.Email = email;
                    UserSession.Password = password1;
                }

                MessageBox.Show("Регистрация успешна!");
                MainWindow.init.OpenPage(new registration_fio(email)); // Переход на страницу регистрации ФИО
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при регистрации: " + ex.Message);
            }
        }

        private void NavigateToFio(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new registration_fio());
        }

        private void NavigateToPhone(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new registration_phone());
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?=.*[_\-!?+=]).{8,}$");
        }

        private void LoadExistingData()
        {
            LoginTextBox.Text = UserSession.Email;
            PasswordBox1.Password = UserSession.Password;
            PasswordBox2.Password = UserSession.Password;
        }

        private void SaveUserData()
        {
            UserSession.Email = LoginTextBox.Text;
            UserSession.Password = PasswordBox1.Password;
            UserSession.Password = PasswordBox2.Password;
        }
    }
}
