using FlowerShop.Classes;
using FlowerShop.Classes.Common;
using MySql.Data.MySqlClient;
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

namespace FlowerShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для products.xaml
    /// </summary>
    public partial class products : Page
    {
        private item_categories currentCategory;

        public products(item_categories category)
        {
            InitializeComponent();
            currentCategory = category;
            LoadProductsFromDatabase();
            ShoppingCart.Instance.CartUpdated += ShoppingCart_CartUpdated;
            UpdateCartSummary();
        }

        private void ShoppingCart_CartUpdated(object sender, EventArgs e)
        {
            UpdateCartSummary();
        }

        private void UpdateCartSummary()
        {
            decimal totalAmount = ShoppingCart.Instance.GetTotalCost();
            string cartButtonText = $"Корзина ({totalAmount} р.)";
            CartButton.Content = cartButtonText;
        }

        private void LoadProductsFromDatabase()
        {
            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = "SELECT ProductID, Name, Price, Src FROM Products WHERE CategoryID = @CategoryID";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CategoryID", currentCategory.CategoryID);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int productId = reader.GetInt32("ProductID");
                        string name = reader.GetString("Name");
                        decimal price = reader.GetDecimal("Price");
                        string src = reader.GetString("Src");

                        var product = new FlowerShop.Elements.item_products(new FlowerShop.Classes.item_products(productId, currentCategory, name, (int)price, src));
                        parent.Children.Add(product);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при загрузке товаров: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new categories());
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
            System.Diagnostics.Process.Start("https://www.youtube.com/watch?v=9vjcZPteWrY");
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new Cart());
        }
    }
}
