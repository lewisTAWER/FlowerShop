using FlowerShop.Classes;
using FlowerShop.Classes.Common;
using MySql.Data.MySqlClient;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using static FlowerShop.App;

namespace FlowerShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для order_form.xaml
    /// </summary>
    public partial class order_form : Page
    {
        public order_form()
        {
            InitializeComponent();
            LoadSavedData();
            UpdateCostDetails();
        }

        private void go_vk(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/syrprizko");
        }

        private void go_telegram(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://web.telegram.org/");
        }

        private void go_instagram(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/teddyflowers_perm/");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Cart());
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidForm())
            {
                var result = MessageBox.Show("Хотите ли вы сохранить введенные данные?", "Подтверждение", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveFormData();
                    SaveFormDataToDatabase();
                }
                SaveOrderToDatabase();
                MessageBox.Show("Заказ успешно оформлен!");
                ShoppingCart.Instance.Clear();
                MainWindow.init.OpenPage(new categories());
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно.");
            }
        }

        private bool IsValidForm()
        {
            if (string.IsNullOrWhiteSpace(LastName.Text) || LastName.Text.Length > 20 || !Regex.IsMatch(LastName.Text, @"^[а-яА-Я]+$")) return false;
            if (string.IsNullOrWhiteSpace(FirstName.Text) || FirstName.Text.Length > 20 || !Regex.IsMatch(FirstName.Text, @"^[а-яА-Я]+$")) return false;
            if (string.IsNullOrWhiteSpace(MiddleName.Text) || MiddleName.Text.Length > 20 || !Regex.IsMatch(MiddleName.Text, @"^[а-яА-Я]+$")) return false;
            if (string.IsNullOrWhiteSpace(Phone.Text) || !Regex.IsMatch(Phone.Text, @"^(\+7|8)\d{10}$")) return false;
            if (string.IsNullOrWhiteSpace(Email.Text) || !Regex.IsMatch(Email.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return false;
            if (string.IsNullOrWhiteSpace(Street.Text) || Street.Text.Length > 30 || !Regex.IsMatch(Street.Text, @"^[а-яА-Я0-9\s]+$")) return false;
            if (string.IsNullOrWhiteSpace(HouseNumber.Text) || HouseNumber.Text.Length > 5 || !Regex.IsMatch(HouseNumber.Text, @"^\d+$")) return false;
            if (string.IsNullOrWhiteSpace(ApartmentNumber.Text) || ApartmentNumber.Text.Length > 5 || !Regex.IsMatch(ApartmentNumber.Text, @"^\d+$")) return false;
            if (!string.IsNullOrWhiteSpace(EntranceNumber.Text) && !Regex.IsMatch(EntranceNumber.Text, @"^\d{1,5}$")) return false;
            if (!string.IsNullOrWhiteSpace(FloorNumber.Text) && !Regex.IsMatch(FloorNumber.Text, @"^\d{1,5}$")) return false;

            return true;
        }

        private void LoadSavedData()
        {
            LastName.Text = UserSession.LastName;
            FirstName.Text = UserSession.FirstName;
            MiddleName.Text = UserSession.MiddleName;
            Phone.Text = UserSession.Phone;
            Email.Text = UserSession.Email;
            Street.Text = UserSession.Street;
            HouseNumber.Text = UserSession.HouseNumber;
            ApartmentNumber.Text = UserSession.ApartmentNumber;
            EntranceNumber.Text = UserSession.EntranceNumber;
            FloorNumber.Text = UserSession.FloorNumber;
        }

        private void SaveFormData()
        {
            UserSession.LastName = LastName.Text;
            UserSession.FirstName = FirstName.Text;
            UserSession.MiddleName = MiddleName.Text;
            UserSession.Phone = Phone.Text;
            UserSession.Email = Email.Text;
            UserSession.Street = Street.Text;
            UserSession.HouseNumber = HouseNumber.Text;
            UserSession.ApartmentNumber = ApartmentNumber.Text;
            UserSession.EntranceNumber = EntranceNumber.Text;
            UserSession.FloorNumber = FloorNumber.Text;
        }

        private void SaveOrderToDatabase()
        {
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    // Сохранение заказа
                    string insertOrderQuery = @"
                        INSERT INTO Orders (UserID, TotalAmount, Discount, FinalAmount)
                        VALUES (@UserID, @TotalAmount, @Discount, @FinalAmount)";

                    MySqlCommand cmd = new MySqlCommand(insertOrderQuery, connection);
                    cmd.Parameters.AddWithValue("@UserID", UserSession.UserID);
                    cmd.Parameters.AddWithValue("@TotalAmount", ShoppingCart.Instance.GetTotalCost());
                    cmd.Parameters.AddWithValue("@Discount", 0);
                    cmd.Parameters.AddWithValue("@FinalAmount", ShoppingCart.Instance.GetTotalCost());

                    cmd.ExecuteNonQuery();
                    int orderId = (int)cmd.LastInsertedId;

                    // Сохранение элементов заказа
                    foreach (var item in ShoppingCart.Instance.CartItems)
                    {
                        string insertOrderItemQuery = @"
                            INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price)
                            VALUES (@OrderID, @ProductID, @Quantity, @Price)";

                        MySqlCommand itemCmd = new MySqlCommand(insertOrderItemQuery, connection);
                        itemCmd.Parameters.AddWithValue("@OrderID", orderId);
                        itemCmd.Parameters.AddWithValue("@ProductID", item.Item.ProductID);
                        itemCmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                        itemCmd.Parameters.AddWithValue("@Price", item.Item.ProductsPrice);

                        itemCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении заказа: " + ex.Message);
            }
        }

        private void SaveFormDataToDatabase()
        {
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = @"
                        UPDATE Users SET
                        LastName = @LastName,
                        FirstName = @FirstName,
                        MiddleName = @MiddleName,
                        PhoneNumber = @PhoneNumber,
                        Email = @Email,
                        Street = @Street,
                        HouseNumber = @HouseNumber,
                        ApartmentNumber = @ApartmentNumber,
                        EntranceNumber = @EntranceNumber,
                        FloorNumber = @FloorNumber WHERE UserId = " + UserSession.UserID;

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@LastName", LastName.Text);
                    cmd.Parameters.AddWithValue("@FirstName", FirstName.Text);
                    cmd.Parameters.AddWithValue("@MiddleName", MiddleName.Text);
                    cmd.Parameters.AddWithValue("@PhoneNumber", Phone.Text);
                    cmd.Parameters.AddWithValue("@Email", Email.Text);
                    cmd.Parameters.AddWithValue("@Street", Street.Text);
                    cmd.Parameters.AddWithValue("@HouseNumber", HouseNumber.Text);
                    cmd.Parameters.AddWithValue("@ApartmentNumber", ApartmentNumber.Text);
                    cmd.Parameters.AddWithValue("@EntranceNumber", EntranceNumber.Text);
                    cmd.Parameters.AddWithValue("@FloorNumber", FloorNumber.Text);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных в базу данных: " + ex.Message);
            }
        }

        private void UpdateCostDetails()
        {
            TotalCost.Text = $"Стоимость: {ShoppingCart.Instance.GetTotalCost()} рублей";
            Discount.Text = $"Скидка: 0 рублей"; // Измените логику расчета скидки по необходимости
            FinalCost.Text = $"Итого: {ShoppingCart.Instance.GetTotalCost()} рублей";
        }
    }
}
