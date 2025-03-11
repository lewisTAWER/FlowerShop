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
    /// Логика взаимодействия для registration_fio.xaml
    /// </summary>
    public partial class registration_fio : Page
    {
        private string userEmail;

        public registration_fio()
        {
            InitializeComponent();
            LoadExistingData();
        }

        public registration_fio(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadExistingData();
        }

        private void registration_fio_back(object sender, RoutedEventArgs e)
        {
            SaveUserData();
            MainWindow.init.OpenPage(new registration_login());
        }

        private void registration_fio_next(object sender, RoutedEventArgs e)
        {
            SaveUserData();

            if (!IsValidName(UserSession.LastName) || !IsValidName(UserSession.FirstName) || !IsValidName(UserSession.MiddleName))
            {
                MessageBox.Show("Вводите только текстовые буквы русского или латинского языка.");
                return;
            }

            // Обновление данных пользователя в базе данных
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = "UPDATE Users SET LastName = @LastName, FirstName = @FirstName, MiddleName = @MiddleName WHERE Email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@LastName", string.IsNullOrEmpty(UserSession.LastName) ? (object)DBNull.Value : UserSession.LastName);
                    cmd.Parameters.AddWithValue("@FirstName", string.IsNullOrEmpty(UserSession.FirstName) ? (object)DBNull.Value : UserSession.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", string.IsNullOrEmpty(UserSession.MiddleName) ? (object)DBNull.Value : UserSession.MiddleName);
                    cmd.Parameters.AddWithValue("@Email", UserSession.Email);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Данные сохранены успешно!");
                        MainWindow.init.OpenPage(new registration_phone(UserSession.Email)); // Передача email
                    }
                    else
                    {
                        MessageBox.Show("Не удалось обновить данные. Проверьте введенный email.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении данных: " + ex.Message);
            }
        }

        private void NavigateToLogin(object sender, RoutedEventArgs e)
        {
            SaveUserData();
            MainWindow.init.OpenPage(new registration_login());
        }

        private void NavigateToPhone(object sender, RoutedEventArgs e)
        {
            SaveUserData();
            MainWindow.init.OpenPage(new registration_phone(UserSession.Email)); // Передача email
        }

        private void LoadExistingData()
        {
            LastNameTextBox.Text = UserSession.LastName;
            FirstNameTextBox.Text = UserSession.FirstName;
            MiddleNameTextBox.Text = UserSession.MiddleName;
        }

        private void SaveUserData()
        {
            UserSession.LastName = LastNameTextBox.Text;
            UserSession.FirstName = FirstNameTextBox.Text;
            UserSession.MiddleName = MiddleNameTextBox.Text;
        }

        private bool IsValidName(string name)
        {
            return string.IsNullOrEmpty(name) || Regex.IsMatch(name, @"^[a-zA-Zа-яА-Я]*$");
        }
    }
}
