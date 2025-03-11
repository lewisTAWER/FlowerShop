using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlowerShop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static class UserSession
        {
            public static string UserID { get; set; }
            public static string Email { get; set; }
            public static string Password { get; set; }
            public static string LastName { get; set; }
            public static string FirstName { get; set; }
            public static string MiddleName { get; set; }
            public static string Phone { get; set; }
            public static string BirthDate { get; set; }
            public static string Gender { get; set; }
            public static string Street { get; set; }
            public static string HouseNumber { get; set; }
            public static string ApartmentNumber { get; set; }
            public static string EntranceNumber { get; set; }
            public static string FloorNumber { get; set; }

            public static void Clear()
            {
                Email = null;
                LastName = null;
                FirstName = null;
                MiddleName = null;
                Phone = null;
                BirthDate = null;
                Gender = null;
                Street = null;
                HouseNumber = null;
                ApartmentNumber = null;
                EntranceNumber = null;
                FloorNumber = null;
            }
        }




    }
}
