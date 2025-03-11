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
using System.Xml.Linq;

namespace FlowerShop.Pages
{
    /// <summary>
    /// Логика взаимодействия для categories.xaml
    /// </summary>
    public partial class categories : Page
    {
        public List<item_categories> items_categories = new List<item_categories>();

        public categories()
        {
            InitializeComponent();
            LoadCategoriesFromDatabase();
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

        public void LoadCategoriesFromDatabase()
        {
            items_categories.Clear();

            try
            {
                using (MySqlConnection connection = DBConnection.OpenConnection())
                {
                    string query = "SELECT CategoryID, Name, Src FROM Categories";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        int categoryID = reader.GetInt32("CategoryID");
                        string name = reader.GetString("Name");
                        string src = reader.GetString("Src");

                        items_categories.Add(new item_categories(categoryID, name, src));
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при загрузке категорий: " + ex.Message);
            }

            LoadItemCategories();
        }

        public void LoadItemCategories()
        {
            parent.Children.Clear();

            foreach (var item in items_categories)
            {
                parent.Children.Add(new Elements.item_categories(item));
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Clear();
            MainWindow.init.OpenPage(new authorization());
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
