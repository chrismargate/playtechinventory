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
        public List<String> itemOptions { get; set; }
        public List<String> customerOptions { get; set; }
        public List<String> supplierOptions { get; set; }

        public TransactionAdd()
        {
            InitializeComponent();
            populateItemOptions();
            populateCustomerOptions();
            populateSupplierOptions();
            transaction_types = new string[] { "PURCHASE", "SALE" };
            DataContext = this;
        }

        private void populateItemOptions()
        {

            List<String> tempItemsOptions = new List<String>();
            
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM items_t", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tempItemsOptions.Add(String.Format("{0} - {1}",reader.GetString("item_id"),reader.GetString("item_name")));
                }

                itemOptions = tempItemsOptions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}",ex));
            }
            finally
            {
                conn.Close();
            }
        }

        private void populateCustomerOptions()
        {

            List<String> tempCustomerOptions = new List<String>();

            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM customers_t", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tempCustomerOptions.Add(String.Format("{0} - {1}", reader.GetString("customer_id"), reader.GetString("fullname")));
                }

                customerOptions = tempCustomerOptions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}", ex));
            }
            finally
            {
                conn.Close();
            }
        }

        private void populateSupplierOptions()
        {
            List<String> tempSupplierOptions = new List<String>();

            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM suppliers_t", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tempSupplierOptions.Add(String.Format("{0} - {1}", reader.GetString("supplier_id"), reader.GetString("fullname")));
                }

                supplierOptions = tempSupplierOptions;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}", ex));
            }
            finally
            {
                conn.Close();
            }
        }

        private bool updateItem()
        {
            int item_id = Convert.ToInt32(cbItem.SelectedItem.ToString().Split(' ')[0]);
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            string findItemQuantity = "SELECT quantity FROM items_t WHERE item_id=@item_id";
            MySqlCommand qtyCmd = new MySqlCommand(findItemQuantity, conn);
            qtyCmd.Parameters.AddWithValue("@item_id", item_id);
            MySqlDataReader reader = qtyCmd.ExecuteReader();
            reader.Read();

            int previousQuantity = reader.GetInt32("quantity");


            conn.Close();

            try
            {

                conn.Open();

                int inputQuantity = Convert.ToInt32(tbQuantity.Text);

                if (inputQuantity <= 0)
                {
                    MessageBox.Show("Please enter a quantity greater than 0");
                    throw new Exception();
                }

                int newQuantity;

                if (cbTransactionType.SelectedItem.ToString() == "PURCHASE")
                {
                    newQuantity = previousQuantity + inputQuantity;
                }
                else
                {
                    newQuantity = previousQuantity - inputQuantity;
                    if (newQuantity < 0)
                    {
                        MessageBox.Show("Insufficient remaining quantity. Please try again!");
                        throw new Exception();
                    }
                }
                
                MySqlCommand cmd = new MySqlCommand("UPDATE items_T SET quantity=@quantity WHERE item_id=@item_id", conn);

                cmd.Parameters.AddWithValue("@item_id", item_id);
                cmd.Parameters.AddWithValue("@quantity", newQuantity);

                cmd.ExecuteNonQuery();
                MessageBox.Show("You have successfully added a transaction!", "Transaction Added");
                return true;
                this.Close();

            }
            catch (FormatException fe)
            {
                MessageBox.Show("Oops! You have entered a letter on either the quantity. Please try again!", "Incorrect Input");
                return false;
            }
            catch (Exception ex)
            {
                // MessageBox.Show(String.Format("{0}", ex));
                return false;
            }

            finally
            {
                conn.Close();
            }
            
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

                int item_id = Convert.ToInt32(cbItem.SelectedItem.ToString().Split(' ')[0]);
                string transaction_type = cbTransactionType.SelectedItem.ToString();
                int? customer_id = 0;
                int? supplier_id = 0;

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
                    // supplier_id = Convert.ToInt32(tbSupplierID.Text);
                    supplier_id = Convert.ToInt32(cbSupplier.SelectedItem.ToString().Split(' ')[0]);
                    customer_id = null;
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
                    //customer_id = Convert.ToInt32(tbCustomerID.Text);
                    customer_id = Convert.ToInt32(cbCustomer.SelectedItem.ToString().Split(' ')[0]);
                    supplier_id = null;
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
                // MessageBox.Show(insertCommand.ToString());

                if (updateItem() == true)
                {
                    insertCommand.ExecuteNonQuery();
                    this.Close();
                }
                // clearTextBoxes();
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Oops! Wrong input. Please try again!", "Incorrect Input");
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}", ex));
            }
            finally
            {
                conn.Close();
            }
        }

        private void clearTextBoxes()
        {
            tbQuantity.Text = "";
            tbPrice.Text = "";
            tbItemDescription.Text = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            ItemAdd itemAdd = new ItemAdd();
            itemAdd.Show();
            this.Close();
        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerAdd customerAdd = new CustomerAdd();
            customerAdd.Show();
            this.Close();
        }

        private void btnAddSupplier_Click(object sender, RoutedEventArgs e)
        {
            SupplierAdd supplierAdd = new SupplierAdd();
            supplierAdd.Show();
            this.Close();
        }

        private void cbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void cbTransactionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTransactionType.SelectedItem.ToString() == "PURCHASE")
            {
                cbCustomer.IsEnabled = false;
                cbSupplier.IsEnabled = true;
            }
            else
            {
                cbCustomer.IsEnabled = true;
                cbSupplier.IsEnabled = false;
            }
        }

        private void cbCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tbQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                if(tbQuantity.Text == "")
                {
                    tbPrice.Text = "";
                    conn.Close();
                    return;
                }else if (cbItem.SelectedItem.ToString() == "")
                {
                    conn.Close();
                    return;
                }

                int item_id = Convert.ToInt32(cbItem.SelectedItem.ToString().Split(' ')[0]);
                int quantity = Convert.ToInt32(tbQuantity.Text);

                MySqlCommand findItemPrice = new MySqlCommand("SELECT price FROM items_t WHERE item_id=@item_id", conn);
                findItemPrice.Parameters.AddWithValue("@item_id", item_id);
                MySqlDataReader itemReader = findItemPrice.ExecuteReader();
                itemReader.Read();

                double totalPrice = itemReader.GetDouble("price") * quantity;
                tbPrice.Text = totalPrice.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}",ex));
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
