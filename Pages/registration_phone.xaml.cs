using FlowerShop.Classes.Common;
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
using static FlowerShop.App;

namespace FlowerShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для registration_phone.xaml
    /// </summary>
    public partial class registration_phone : Page
    {
        private string userEmail;

        public registration_phone()
        {
            InitializeComponent();
            userEmail = UserSession.Email; // Получение email из глобального состояния
            LoadExistingData();
        }

        public registration_phone(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadExistingData();
        }

        private void registration_phone_back(object sender, RoutedEventArgs e)
        {
            SaveUserData();
            MainWindow.init.OpenPage(new registration_fio(userEmail)); // Передача email при возврате
        }

        private void registration_phone_next(object sender, RoutedEventArgs e)
        {
            SaveUserData();

            if (!IsValidPhone(UserSession.Phone))
            {
                MessageBox.Show("Введите корректный номер телефона.");
                return;
            }

            if (!IsValidBirthDate(UserSession.BirthDate))
            {
                MessageBox.Show("Выберите корректную дату рождения.");
                return;
            }

            if (string.IsNullOrEmpty(UserSession.Gender))
            {
                MessageBox.Show("Пожалуйста, выберите пол.");
                return;
            }

            // Обновление данных пользователя в базе данных
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = "UPDATE Users SET PhoneNumber = @PhoneNumber, BirthDate = @BirthDate, Gender = @Gender WHERE Email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@PhoneNumber", UserSession.Phone);
                    cmd.Parameters.AddWithValue("@BirthDate", DateTime.ParseExact(UserSession.BirthDate, "dd.MM.yyyy", null).ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@Gender", UserSession.Gender);
                    cmd.Parameters.AddWithValue("@Email", UserSession.Email);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Данные сохранены успешно!");
                        MainWindow.init.OpenPage(new registration_adress(UserSession.Email)); // Переход на страницу регистрации адреса
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

        private void NavigateToFio(object sender, RoutedEventArgs e)
        {
            SaveUserData();
            MainWindow.init.OpenPage(new registration_fio(userEmail)); // Передача email при возврате
        }

        private void LoadExistingData()
        {
            PhoneTextBox.Text = UserSession.Phone;
            if (DateTime.TryParse(UserSession.BirthDate, out DateTime birthDate))
            {
                BirthDatePicker.SelectedDate = birthDate;
            }
            if (UserSession.Gender == "Мужской")
            {
                MaleRadioButton.IsChecked = true;
            }
            else if (UserSession.Gender == "Женский")
            { 
                FemaleRadioButton.IsChecked = true;
            }
        }

        private void SaveUserData()
        {
            UserSession.Phone = PhoneTextBox.Text;
            UserSession.BirthDate = BirthDatePicker.SelectedDate?.ToString("dd.MM.yyyy");
            UserSession.Gender = MaleRadioButton.IsChecked == true ? "Мужской" : FemaleRadioButton.IsChecked == true ? "Женский" : null;
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^(\+7|8)\d{10}$");
        }

        private bool IsValidBirthDate(string birthDate)
        {
            return DateTime.TryParseExact(birthDate, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }
    }
}
