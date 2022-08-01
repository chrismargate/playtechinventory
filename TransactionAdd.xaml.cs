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

    public partial class TransactionAdd : Window
    {

        private const string DBSERVER = "localhost";
        private const string DBNAME = "db.playtech";
        private const string DBUSER = "chris";
        private const string DBPASS = "12345";
        public string[] transaction_types { get; set; }
        public TransactionAdd()
        {
            InitializeComponent();
            transaction_types = new string[] { "PURCHASE", "SALE" };
            DataContext = this;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string findItemQuery = "SELECT COUNT(*) FROM items_T WHERE item_id=@item_id";
                string findCustomerQuery = "SELECT COUNT(*) FROM customers_T WHERE customer_id=@customer_id";
                string findSupplierQuery = "SELECT COUNT(*) FROM suppliers_T WHERE supplier_id=@supplier_id";

                int item_id = Convert.ToInt32(tbItemID.Text);
                string transaction_type = comboBox1.SelectedItem.ToString();
                int customer_id = 0;
                int supplier_id = 0;

                MySqlCommand findItemCommand = new MySqlCommand(findItemQuery, conn);
                findItemCommand.Parameters.AddWithValue("@item_id", item_id);
                MySqlDataReader itemReader = findItemCommand.ExecuteReader();
                itemReader.Read();

                if (itemReader.GetInt32("COUNT(*)") <= 0)
                {
                    MessageBox.Show("Please enter a valid item ID", "Incorrect Item ID");
                    throw new Exception();
                }
                itemReader.Close();

                if (transaction_type == "PURCHASE")
                {
                    supplier_id = Convert.ToInt32(tbSupplierID.Text);
                    customer_id = 0;
                    MySqlCommand findSupplierCommand = new MySqlCommand(findSupplierQuery, conn);
                    findSupplierCommand.Parameters.AddWithValue("@supplier_id", supplier_id);
                    MySqlDataReader supplierReader = findSupplierCommand.ExecuteReader();
                    supplierReader.Read();

                    if (supplierReader.GetInt32("COUNT(*)") <= 0)
                    {
                        MessageBox.Show("Please enter a valid supplier ID", "Incorrect Supplier ID");
                        throw new Exception();
                    }
                    supplierReader.Close();
                }
                else
                {
                    customer_id = Convert.ToInt32(tbCustomerID.Text);
                    supplier_id = 0;
                    MySqlCommand findCustomerCommand = new MySqlCommand(findCustomerQuery, conn);
                    findCustomerCommand.Parameters.AddWithValue("@customer_id", customer_id);
                    MySqlDataReader customerReader = findCustomerCommand.ExecuteReader();
                    customerReader.Read();

                    if (customerReader.GetInt32("COUNT(*)") <= 0)
                    {
                        MessageBox.Show("Please enter a valid customer ID", "Incorrect Customer ID");
                        throw new Exception();
                    }
                    customerReader.Close();
                }

                int quantity = Convert.ToInt32(tbQuantity.Text);
                double price = Convert.ToDouble(tbPrice.Text);
                string description = tbItemDescription.Text;


                if (quantity <= 0)
                {
                    MessageBox.Show("Please enter a quantity greater than 0");
                    throw new Exception();
                }

                if (price <= 0)
                {
                    MessageBox.Show("Please enter a price greater than 0");
                    throw new Exception();
                }

                MySqlCommand insertCommand = new MySqlCommand("INSERT INTO transactions_T(item_id, transaction_type, customer_id, supplier_id, description, total_qty, total_price) VALUES (@item_id,@transaction_type,@customer_id,@supplier_id,@description,@total_qty,@total_price)", conn);
                insertCommand.Parameters.AddWithValue("@item_id", item_id);
                insertCommand.Parameters.AddWithValue("@transaction_type", transaction_type);
                insertCommand.Parameters.AddWithValue("@customer_id", customer_id);
                insertCommand.Parameters.AddWithValue("@supplier_id", supplier_id);
                insertCommand.Parameters.AddWithValue("@description", description);
                insertCommand.Parameters.AddWithValue("@total_qty", quantity);
                insertCommand.Parameters.AddWithValue("@total_price", price);
                insertCommand.ExecuteNonQuery();

                MessageBox.Show("You have successfully added a transaction!", "Transaction Added");
                this.Close();
                // clearTextBoxes();
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Oops! Wrong input. Please try again!", "Incorrect Input");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(String.Format("{0}", ex));
            }
            finally
            {
                conn.Close();
            }
        }

        private void clearTextBoxes()
        {
            tbItemID.Text = "";
            tbCustomerID.Text = "";
            tbSupplierID.Text = "";
            tbQuantity.Text = "";
            tbPrice.Text = "";
            tbItemDescription.Text = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "PURCHASE")
            {
                tbCustomerID.IsEnabled = false;
                tbSupplierID.IsEnabled = true;
            }
            else
            {
                tbCustomerID.IsEnabled = true;
                tbSupplierID.IsEnabled = false;
            }
        }
    }
}
