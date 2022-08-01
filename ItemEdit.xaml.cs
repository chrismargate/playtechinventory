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
using MySql.Data.MySqlClient;

namespace PlayTechInventory
{
    /// <summary>
    /// Interaction logic for ItemAdd.xaml
    /// </summary>
    public partial class ItemEdit : Window
    {
        private const string DBSERVER = "localhost";
        private const string DBNAME = "db.playtech";
        private const string DBUSER = "chris";
        private const string DBPASS = "12345";
        private int itemid;
        public ItemEdit(int item_id)
        {
            InitializeComponent();
            itemid = item_id;
            fillTextBoxes();
        }

        private void fillTextBoxes()
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM items_T WHERE item_id=@item_id", conn);
                cmd.Parameters.AddWithValue("@item_id", this.itemid);

                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                tbItemID.Text = reader.GetString("item_id");
                tbItemName.Text = reader.GetString("item_name");
                tbItemCategory.Text = reader.GetString("category");
                tbItemDescription.Text = reader.GetString("description");
                tbItemQty.Text = reader.GetString("quantity");
                tbItemPrice.Text = reader.GetString("price");

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
            tbItemID.Text = "";
            tbItemName.Text = "";
            tbItemCategory.Text = "";
            tbItemDescription.Text = "";
            tbItemQty.Text = "";
            tbItemPrice.Text = "";
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            try
            {
                string item_name = tbItemName.Text;
                string category = tbItemCategory.Text;
                string description = tbItemDescription.Text;
                int quantity = Convert.ToInt32(tbItemQty.Text);
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

                if (category.Length <= 0)
                {
                    MessageBox.Show("You have not entered a category. Please try again!");
                    throw new Exception();
                }

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

                MySqlCommand cmd = new MySqlCommand("UPDATE items_T SET item_name=@item_name, category=@category, description=@description, quantity=@quantity,price=@price WHERE item_id=@item_id", conn);

                cmd.Parameters.AddWithValue("@item_id", this.itemid);
                cmd.Parameters.AddWithValue("@item_name", item_name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@price", price);

                cmd.ExecuteNonQuery();

                MessageBox.Show("You have successfully edited an item!","Item Edited");
                this.Close();
                
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Oops! You have entered a letter on either the quantity or the price. Please try again!","Incorrect Input");
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

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
