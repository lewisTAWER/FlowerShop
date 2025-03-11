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
    /// Логика взаимодействия для registration_adress.xaml
    /// </summary>
    public partial class registration_adress : Page
    {
        private string userEmail;

        public registration_adress()
        {
            InitializeComponent();
            userEmail = UserSession.Email; // Получение email из глобального состояния
            LoadExistingData();
        }

        public registration_adress(string email)
        {
            InitializeComponent();
            userEmail = email;
            LoadExistingData();
        }

        private void registration_adress_back(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new registration_phone(userEmail));
        }

        private void registration_adress_next(object sender, RoutedEventArgs e)
        {
            SaveUserData();

            if (!IsValidEntranceOrFloor(UserSession.EntranceNumber) || !IsValidEntranceOrFloor(UserSession.FloorNumber))
            {
                MessageBox.Show("Номер подъезда и номер этажа должны быть числовыми значениями.");
                return;
            }

            // Обновление данных пользователя в базе данных
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = "UPDATE Users SET Street = @Street, HouseNumber = @HouseNumber, ApartmentNumber = @ApartmentNumber, EntranceNumber = @EntranceNumber, FloorNumber = @FloorNumber WHERE Email = @Email";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Street", UserSession.Street);
                    cmd.Parameters.AddWithValue("@HouseNumber", UserSession.HouseNumber);
                    cmd.Parameters.AddWithValue("@ApartmentNumber", UserSession.ApartmentNumber);
                    cmd.Parameters.AddWithValue("@EntranceNumber", string.IsNullOrEmpty(UserSession.EntranceNumber) ? (object)DBNull.Value : UserSession.EntranceNumber);
                    cmd.Parameters.AddWithValue("@FloorNumber", string.IsNullOrEmpty(UserSession.FloorNumber) ? (object)DBNull.Value : UserSession.FloorNumber);
                    cmd.Parameters.AddWithValue("@Email", UserSession.Email);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Данные сохранены успешно!");

                        using (MySqlConnection connection1 = DBConnection.OpenConnection())
                        {
                            query = "SELECT * FROM Users WHERE Email = @Email";
                            cmd = new MySqlCommand(query, connection1);

                            cmd.Parameters.AddWithValue("@Email", UserSession.Email);

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
                                    UserSession.BirthDate = reader["BirthDate"].ToString();
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

        private void LoadExistingData()
        {
            StreetTextBox.Text = UserSession.Street;
            HouseNumberTextBox.Text = UserSession.HouseNumber;
            ApartmentNumberTextBox.Text = UserSession.ApartmentNumber;
            EntranceNumberTextBox.Text = UserSession.EntranceNumber;
            FloorNumberTextBox.Text = UserSession.FloorNumber;
        }

        private void SaveUserData()
        {
            UserSession.Street = StreetTextBox.Text;
            UserSession.HouseNumber = HouseNumberTextBox.Text;
            UserSession.ApartmentNumber = ApartmentNumberTextBox.Text;
            UserSession.EntranceNumber = EntranceNumberTextBox.Text;
            UserSession.FloorNumber = FloorNumberTextBox.Text;
        }

        private bool IsValidEntranceOrFloor(string value)
        {
            return string.IsNullOrEmpty(value) || Regex.IsMatch(value, @"^\d+$");
        }
    }
}
