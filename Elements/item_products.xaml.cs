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
    /// Логика взаимодействия для item_products.xaml
    /// </summary>
    public partial class item_products : UserControl
    {
        private Classes.item_products ThisProduct { get; set; }
        private bool isInCart = false;

        public item_products(Classes.item_products item)
        {
            InitializeComponent();
            if (item != null)
            {
                ThisProduct = item;
                string imagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"Image\Products", ThisProduct.Category.CategoriesName, item.Src);
                if (File.Exists(imagePath))
                {
                    products_image.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    products_image.Source = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Image/Chamomile.png")));
                }
                products_name.Content = item.ProductsName;
                products_price.Content = $"{item.ProductsPrice} р.";
                products_count.Text = "0";

                ShoppingCart.Instance.CartUpdated += OnCartUpdated;

                // Проверка состояния корзины
                CheckCartState();
            }
        }

        private void OnCartUpdated(object sender, EventArgs e)
        {
            CheckCartState();
        }

        private void CheckCartState()
        {
            var existingItem = ShoppingCart.Instance.CartItems.FirstOrDefault(ci => ci.Item.ProductsName == ThisProduct.ProductsName);
            if (existingItem != null)
            {
                products_count.Text = existingItem.Quantity.ToString();
                isInCart = true;
                products_AddToCart.IsEnabled = false;
            }
            else
            {
                products_count.Text = "0";
                isInCart = false;
                products_AddToCart.IsEnabled = true;
            }
        }

        private void products_minus(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(products_count.Text);
            if (count > 0)
            {
                count--;
                products_count.Text = count.ToString();
                if (isInCart)
                {
                    if (count == 0)
                    {
                        if (MessageBox.Show("Вы действительно хотите удалить этот товар из корзины?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            ShoppingCart.Instance.AddItem(ThisProduct, count);
                            MessageBox.Show($"{ThisProduct.ProductsName} удалён из корзины.");
                        }
                        else
                        {
                            // Если пользователь отменяет удаление, вернуть количество на 1
                            count++;
                            products_count.Text = count.ToString();
                        }
                    }
                    else
                    {
                        ShoppingCart.Instance.AddItem(ThisProduct, count);
                    }
                }
            }
        }

        private void products_plus(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(products_count.Text);
            count++;
            products_count.Text = count.ToString();
            if (isInCart)
            {
                ShoppingCart.Instance.AddItem(ThisProduct, count);
            }
        }

        private void OnAddToCartClick(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(products_count.Text);
            if (count > 0)
            {
                ShoppingCart.Instance.AddItem(ThisProduct, count);
                products_AddToCart.IsEnabled = false;
                isInCart = true;
                MessageBox.Show($"{count} шт. {ThisProduct.ProductsName} добавлены в корзину.");
                CheckCartState(); // Обновить состояние корзины после добавления товара
            }
            else
            {
                MessageBox.Show("Количество товара должно быть больше нуля.");
            }
        }

        private void products_count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }
    }
}
