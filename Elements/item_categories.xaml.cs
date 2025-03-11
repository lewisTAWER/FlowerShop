using System;
using System.Collections.Generic;
using System.IO;
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

namespace FlowerShop.Elements
{
    /// <summary>
    /// Логика взаимодействия для item_categories.xaml
    /// </summary>
    public partial class item_categories : UserControl
    {
        private FlowerShop.Classes.item_categories ThisCategories { get; set; }
        public item_categories(FlowerShop.Classes.item_categories item)
        {
            InitializeComponent();
            if (item != null)
            {
                ThisCategories = item;
                string imagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Image", item.Src);
                if (File.Exists(imagePath))
                {
                    categories_image.Source = new BitmapImage(new Uri(imagePath));
                }
                else
                {
                    categories_image.Source = new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Image/Chamomile.png")));
                }
                categories_name.Content = item.CategoriesName;
            }
        }

        private void To_Products(object sender, MouseButtonEventArgs e)
        {
            MainWindow.init.OpenPage(new FlowerShop.Pages.products(ThisCategories));
        }
    }
}
