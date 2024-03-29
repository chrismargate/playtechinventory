﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace PlayTechInventory
{
    /// <summary>
    /// Interaction logic for SupplierAdd.xaml
    /// </summary>
    public partial class SupplierAdd : Window
    {
        private const string DBSERVER = "localhost";
        private const string DBNAME = "db.playtech";
        private const string DBUSER = "chris";
        private const string DBPASS = "12345";
        public SupplierAdd()
        {
            InitializeComponent();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {

                string fullname = tbSupplierName.Text;
                string email_address = tbSupplierEmail.Text;
                string contact_number = tbSupplierContact.Text;

                if (fullname.Length <= 0)
                {
                    MessageBox.Show("You have not entered a name.");
                    throw new Exception();
                }

                if (email_address.Length <= 0)
                {
                    MessageBox.Show("You have not entered an email address.");
                    throw new Exception();
                }

                if (contact_number.Length > 11)
                {
                    MessageBox.Show("Contact number has exceeded 11 digits");
                    throw new Exception();
                }
                else if (contact_number.Length <= 0)
                {
                    MessageBox.Show("You have not entered a contact number");
                    throw new Exception();
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO suppliers_T(fullname, email_address, contact_number) VALUES (@fullname,@email_address,@contact_number)", conn);
                cmd.Parameters.AddWithValue("@fullname", fullname);
                cmd.Parameters.AddWithValue("@email_address", email_address);
                cmd.Parameters.AddWithValue("@contact_number", contact_number);
                cmd.ExecuteNonQuery();

                MessageBox.Show("You have successfully added a supplier!","Supplier Added");
                this.Close();
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Oops! Wrong input. Please try again!", "Incorrect Input");
            }
            catch (Exception ex)
            {
                // MessageBox.Show(String.Format("{0}", ex));
            }
            finally
            {
                conn.Close();
            }
        }

        private void clearTextBoxes()
        {
            tbSupplierName.Text = "";
            tbSupplierEmail.Text = "";
            tbSupplierContact.Text = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
