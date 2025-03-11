using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.Classes
{
    public class item_products
    {
        public int ProductID { get; set; }
        public item_categories Category { get; set; }
        public string ProductsName { get; set; }
        public int ProductsPrice { get; set; }
        public string Src { get; set; }

        public item_products(int productID, item_categories category, string productsName, int productsPrice, string src)
        {
            ProductID = productID;
            Category = category;
            ProductsName = productsName;
            ProductsPrice = productsPrice;
            Src = src;
        }
    }
}
