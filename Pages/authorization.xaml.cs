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
    /// Логика взаимодействия для authorization.xaml
    /// </summary>
    public partial class authorization : Page
    {
        public authorization()
        {
            InitializeComponent();
        }

        private void authorization_enter(object sender, RoutedEventArgs e)
        {
            string email = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Пожалуйста, введите корректный email.");
                return;
            }

            // Проверка логина и пароля в базе данных
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            // Сохраняем данные пользователя в сессии
                            UserSession.UserID = reader["UserID"].ToString();
                            UserSession.Email = reader["Email"].ToString();
                            UserSession.FirstName = reader["FirstName"].ToString();
                            UserSession.LastName = reader["LastName"].ToString();
                            UserSession.MiddleName = reader["MiddleName"].ToString();
                            UserSession.Phone = reader["PhoneNumber"].ToString();

                            // Преобразование даты рождения
                            string birthDateString = reader["BirthDate"].ToString();
                            if (!string.IsNullOrEmpty(birthDateString))
                            {
                                try
                                {
                                    UserSession.BirthDate = Convert.ToDateTime(birthDateString).ToString("yyyy-MM-dd");
                                }
                                catch (FormatException)
                                {
                                    UserSession.BirthDate = null; // или установите значение по умолчанию
                                }
                            }
                            else
                            {
                                UserSession.BirthDate = null;
                            }

                            UserSession.Gender = reader["Gender"].ToString();
                            UserSession.Street = reader["Street"].ToString();
                            UserSession.HouseNumber = reader["HouseNumber"].ToString();
                            UserSession.ApartmentNumber = reader["ApartmentNumber"].ToString();
                            UserSession.EntranceNumber = reader["EntranceNumber"].ToString();
                            UserSession.FloorNumber = reader["FloorNumber"].ToString();

                            // Переход на страницу категорий
                            MainWindow.init.OpenPage(new categories());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при авторизации: " + ex.Message);
            }
        }

        private void authorization_registr(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new registration_login());
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
