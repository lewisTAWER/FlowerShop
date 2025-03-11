using FlowerShop.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace FlowerShop.Elements
{
    /// <summary>
    /// Логика взаимодействия для CartItemControl.xaml
    /// </summary>
    public partial class CartItemControl : UserControl
    {
        private CartItem ThisCartItem { get; set; }

        public CartItemControl(CartItem cartItem)
        {
            InitializeComponent();
            ThisCartItem = cartItem;
            InitializeProductDetails();
        }

        private void InitializeProductDetails()
        {
            string imagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Image\Products", ThisCartItem.Item.Category.CategoriesName, ThisCartItem.Item.Src);
            if (File.Exists(imagePath))
            {
                products_image.Source = new BitmapImage(new Uri(imagePath));
            }
            else
            {
                products_image.Source = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Image/Chamomile.png")));
            }

            products_name.Content = ThisCartItem.Item.ProductsName;
            products_price.Content = $"{ThisCartItem.Item.ProductsPrice} р.";
            products_count.Text = ThisCartItem.Quantity.ToString();
        }

        private void products_count_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void products_minus(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(products_count.Text);
            if (count > 0)
            {
                count--;
                if (count == 0)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить этот товар из корзины?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        ShoppingCart.Instance.AddItem(ThisCartItem.Item, count);
                    }
                }
                else
                {
                    ShoppingCart.Instance.AddItem(ThisCartItem.Item, count);
                }
                products_count.Text = count.ToString();
            }
        }

        private void products_plus(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(products_count.Text);
            count++;
            ShoppingCart.Instance.AddItem(ThisCartItem.Item, count);
            products_count.Text = count.ToString();
        }
    }
}
