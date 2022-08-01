using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTechInventory
{
    public class Transaction
    {
        public int transaction_id { get; set; }
        public int item_id { get; set; }
        public string datetime { get; set; }
        public string transaction_type { get; set; }
        public int customer_id { get; set; }
        public int supplier_id { get; set; }
        public string description { get; set; }
        public int total_qty { get; set; }
        public double total_price { get; set; }
    }
}
