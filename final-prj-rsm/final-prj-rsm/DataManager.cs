using System;
using System.Collections.Generic;
using System.Text;

namespace final_prj_rsm
{
    class DataManager
    {
        public static string[,] Users = new string[100, 3];
        public static int UserCount = 0;
        public static int ActiveUserIndex = -1;

        public static string[,] Transactions = new string[1000, 6];
        public static int TransactionCount = 0;

        public static string[] Categories = { "غذا", "حمل و نقل", "صورتحساب‌ها", "خرید", "سرگرمی", "دیگر" };
        public static int CategoryCount = 6;    
    }

}
