using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowerShop.Classes
{
    public class item_categories
    {
        public int CategoryID { get; set; }
        public string CategoriesName { get; set; }
        public string Src { get; set; }

        public item_categories(int categoryID, string categoriesName, string src)
        {
            CategoryID = categoryID;
            CategoriesName = categoriesName;
            Src = src;
        }
    }
}
