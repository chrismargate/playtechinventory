using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTechInventory
{
    public class Item
    {
        /*
        public int item_id { get; set; }
        public string item_name { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        */

        private int item_id;
        private string item_name;
        private string category;
        private string description;
        private int quantity;
        private double price;

        public int ItemID
        {
            get
            {
                return item_id;
            }

            set
            {
                item_id = value;
            }
        }

        public string ItemName
        {
            get
            {
                return item_name;
            }

            set
            {
                item_name = value;
            }
        }

        public string Category
        {
            get
            {
                return category;
            }

            set
            {
                category = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

    }
}
