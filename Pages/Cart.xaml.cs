using FlowerShop.Classes;
using FlowerShop.Elements;
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
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Page
    {
        public Cart()
        {
            InitializeComponent();
            LoadCartItems();
            ShoppingCart.Instance.CartUpdated += ShoppingCart_CartUpdated;
            UpdateCartSummary();
        }

        private void ShoppingCart_CartUpdated(object sender, EventArgs e)
        {
            LoadCartItems();
            UpdateCartSummary();
        }

        private void LoadCartItems()
        {
            parent.Children.Clear();
            foreach (var cartItem in ShoppingCart.Instance.CartItems)
            {
                var productControl = new CartItemControl(cartItem);
                parent.Children.Add(productControl);
            }
        }

        private void UpdateCartSummary()
        {
            decimal totalAmount = ShoppingCart.Instance.GetTotalCost();
            string cartButtonText = $"Оформить ({totalAmount} р.)";
            CartButton.Content = cartButtonText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new categories());
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.init.OpenPage(new order_form());
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
    }
}
