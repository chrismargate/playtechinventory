using System;
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
using System.Data;
using MySql.Data.MySqlClient;

namespace PlayTechInventory
{
    /// <summary>
    /// Interaction logic for ItemAdd.xaml
    /// </summary>
    /// 

    public partial class ItemAdd : Window
    {

        private const string DBSERVER = "localhost";
        private const string DBNAME = "db.playtech";
        private const string DBUSER = "chris";
        private const string DBPASS = "12345";

        public ItemAdd()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {

                string item_name = tbItemName.Text;
                string category = tbItemCategory.Text;
                string description = tbItemDescription.Text;
                // int quantity = Convert.ToInt32(tbItemQty.Text);
                int quantity = 0;
                double price = Convert.ToDouble(tbItemPrice.Text);

                if (item_name.Length > 50)
                {
                    MessageBox.Show("Item Name is too long. Please enter 50 or below characters");
                    throw new Exception();
                }
                else if (item_name.Length <= 0)
                {
                    MessageBox.Show("You have not entered an item name. Please try again!");
                    throw new Exception();
                }

                if(category.Length <= 0)
                {
                    MessageBox.Show("You have not entered a category. Please try again!");
                    throw new Exception();
                }

                /*
                if (quantity <= 0)
                {
                    MessageBox.Show("Please enter a quantity greater than 0");
                    throw new Exception();
                }
                */

                if (price <= 0)
                {
                    MessageBox.Show("Please enter a price greater than 0");
                    throw new Exception();
                }

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("INSERT INTO items_T(item_name, category, description, quantity, price) VALUES (@item_name,@category,@description,@quantity,@price)", conn);
                cmd.Parameters.AddWithValue("@item_name", item_name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.ExecuteNonQuery();

                MessageBox.Show("You have successfully added an item!","Item Added");
                btnClose_Click(new object(), new RoutedEventArgs());
                clearTextBoxes();
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Please enter an integer on the quantity and a demical on the price.","Incorrect Input");
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
            tbItemName.Text = "";
            tbItemCategory.Text = "";
            tbItemDescription.Text = "";
            // tbItemQty.Text = "";
            tbItemPrice.Text = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TransactionAdd transactionAdd = new TransactionAdd();
            transactionAdd.Show();
            this.Close();
        }

    }
}
