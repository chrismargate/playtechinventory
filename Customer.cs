using System;
using System.Collections.Generic;
using System.Text;

namespace PlayTechInventory
{
    public class Customer
    {
        private int customer_id;
        private string fullname;
        private string email_address;
        private string contact_number;

        public int CustomerID
        {
            get
            {
                return customer_id;
            }
            set
            {
                customer_id = value;
            }
        }

        public string FullName
        {
            get
            {
                return fullname;
            }
            set
            {
                fullname = value;
            }
        }

        public string Email
        {
            get
            {
                return email_address;
            }
            set
            {
                email_address = value;
            }
        }
        public string ContactNo
        {
            get
            {
                return contact_number;
            }
            set
            {
                contact_number = value;
            }
        }
    }
}
