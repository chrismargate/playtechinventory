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
using System.Collections.ObjectModel;
using System.Collections;
using MySql.Data.MySqlClient;

namespace PlayTechInventory
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {

        private const string DBSERVER = "localhost";
        private const string DBNAME = "db.playtech";
        private const string DBUSER = "chris";
        private const string DBPASS = "12345";

        public Dashboard(string user)
        {
            InitializeComponent();

            populateItemsDataGrid();
            gridItems.Visibility = Visibility.Visible;

            loggedInUser.Text = user;

        }

        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void populateItemsDataGrid()
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>();

            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM items_t", conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            int counter = 0;
            while (reader.Read())
            {

                items.Add(new Item { ItemID = reader.GetInt32("item_id"), ItemName = reader.GetString("item_name"), Category = reader.GetString("category"), Description = reader.GetString("description"), Quantity = reader.GetInt32("quantity"), Price = reader.GetDouble("price") });

                counter++;
            }

            btnItems.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7B5CD6"));
            btnItems.Foreground = Brushes.White;
            itemsDataGrid.ItemsSource = items;
            conn.Close();
        }

        private void hideItemsDataGrid()
        {
            
            btnItems.ClearValue(Button.BackgroundProperty);
            btnItems.ClearValue(Button.ForegroundProperty);
        }


        private void populateSuppliersDataGrid()
       {
          
           ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();

           string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
           MySqlConnection conn = new MySqlConnection(connectionString);
           conn.Open();
           MySqlCommand cmd = new MySqlCommand("SELECT * FROM suppliers_T", conn);
           MySqlDataReader reader = cmd.ExecuteReader();

           int counter = 0;
           while (reader.Read())
           {
               suppliers.Add(new Supplier { SupplierID = reader.GetInt32("supplier_id"), FullName = reader.GetString("fullname"), Email = reader.GetString("email_address"), ContactNo = reader.GetString("contact_number") });
               counter++;
           }

           btnSuppliers.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7B5CD6"));
           btnSuppliers.Foreground = Brushes.White;

           suppliersDataGrid.ItemsSource = suppliers;

           conn.Close();
       }
        private void hideSuppliersDataGrid()
       {
           btnSuppliers.ClearValue(Button.BackgroundProperty);
           btnSuppliers.ClearValue(Button.ForegroundProperty);
       }


        private void populateCustomersDataGrid()
       {

           ObservableCollection<Customer> customers = new ObservableCollection<Customer>();

           string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
           MySqlConnection conn = new MySqlConnection(connectionString);
           conn.Open();
           MySqlCommand cmd = new MySqlCommand("SELECT * FROM customers_T", conn);
           MySqlDataReader reader = cmd.ExecuteReader();

           int counter = 0;
           while (reader.Read())
           {
               customers.Add(new Customer { CustomerID = reader.GetInt32("customer_id"), FullName = reader.GetString("fullname"), Email = reader.GetString("email_address"), ContactNo = reader.GetString("contact_number") });
               counter++;
           }

           btnCustomers.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7B5CD6"));
           btnCustomers.Foreground = Brushes.White;
           customersDataGrid.ItemsSource = customers;

           conn.Close();
       }

        private void hideCustomersDataGrid()
       {
           
           btnCustomers.ClearValue(Button.BackgroundProperty);
           btnCustomers.ClearValue(Button.ForegroundProperty);
       }


        private void btnItems_Click(object sender, RoutedEventArgs e)
        {
            hideSuppliersDataGrid();
            gridSuppliers.Visibility = Visibility.Hidden;

            hideCustomersDataGrid();
            gridCustomers.Visibility = Visibility.Hidden;

            hideLedgerDataGrid();
            gridLedger.Visibility = Visibility.Hidden;

            populateItemsDataGrid();
            gridItems.Visibility = Visibility.Visible;

        }

        private void btnLedger_Click(object sender, RoutedEventArgs e)
        {
            hideItemsDataGrid();
            gridItems.Visibility = Visibility.Hidden;

            hideCustomersDataGrid();
            gridCustomers.Visibility = Visibility.Hidden;

            hideSuppliersDataGrid();
            gridSuppliers.Visibility = Visibility.Hidden;

            populateLedgerDataGrid();
            gridLedger.Visibility = Visibility.Visible;
        }

        private void btnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            hideItemsDataGrid();
            gridItems.Visibility = Visibility.Hidden;

            hideCustomersDataGrid();
            gridCustomers.Visibility = Visibility.Hidden;

            hideLedgerDataGrid();
            gridLedger.Visibility = Visibility.Hidden;

            populateSuppliersDataGrid();
            gridSuppliers.Visibility = Visibility.Visible;
        }
        private void btnCustomers_Click(object sender, RoutedEventArgs e)
        {
            hideItemsDataGrid();
            gridItems.Visibility = Visibility.Hidden;

            hideSuppliersDataGrid();
            gridSuppliers.Visibility = Visibility.Hidden;

            hideLedgerDataGrid();
            gridLedger.Visibility = Visibility.Hidden;

            populateCustomersDataGrid();
            gridCustomers.Visibility = Visibility.Visible;
        }

        private void btnItemRefresh_Click(object sender, RoutedEventArgs e)
        {
            btnItems_Click(new object(), new RoutedEventArgs());
        }

        private void btnItemAdd_Click(object sender, RoutedEventArgs e)
        {
            ItemAdd itemAdd = new ItemAdd();
            itemAdd.Show();
        }

        private void btnItemEdit_Click(object sender, RoutedEventArgs e)
        {
            Item sampleItem = (Item)itemsDataGrid.SelectedItem;
            ItemEdit itemEdit = new ItemEdit(sampleItem.ItemID);
            itemEdit.Show();

        }

        public void btnItemDelete_Click(object sender, RoutedEventArgs e)
        {

            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                Item sampleItem = (Item)itemsDataGrid.SelectedItem;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM items_t WHERE item_id=@item_id", conn);
                cmd.Parameters.AddWithValue("@item_id",sampleItem.ItemID);
                cmd.ExecuteNonQuery();

            }
            else if (dialogResult == MessageBoxResult.No)
            {
                return;
            }

            conn.Close();

            MessageBox.Show("You have successfully deleted a record!");

            btnItems_Click(new object(),new RoutedEventArgs());
        }

        private void populateLedgerDataGrid()
        {
            ObservableCollection<Transaction> transactions = new ObservableCollection<Transaction>();

            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM transactions_T", conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            int counter = 0;
            while (reader.Read())
            {
                transactions.Add(new Transaction { transaction_id = reader.GetInt32("transaction_id"), item_id = reader.GetInt32("item_id"), datetime = reader.GetString("transaction_date"), transaction_type = reader.GetString("transaction_type"), customer_id = reader.GetInt32("customer_id"), supplier_id = reader.GetInt32("supplier_id"), description = reader.GetString("description"), total_qty = reader.GetInt32("total_qty"), total_price = reader.GetDouble("total_price")});
                counter++;
            }

            btnLedger.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#7B5CD6"));
            btnLedger.Foreground = Brushes.White;

            ledgerDataGrid.ItemsSource = transactions;

            conn.Close();
            
        }

        private void hideLedgerDataGrid()
        {
            btnLedger.ClearValue(Button.BackgroundProperty);
            btnLedger.ClearValue(Button.ForegroundProperty);
        }

        private void btnLedgerRefresh_Click(object sender, RoutedEventArgs e)
        {
            btnLedger_Click(new object(), new RoutedEventArgs());
        }
        private void btnLedgerAdd_Click(object sender, RoutedEventArgs e)
        {
            TransactionAdd transactionAdd = new TransactionAdd();
            transactionAdd.Show();
        }
        private void btnLedgerDelete_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                Transaction sampleTransaction = (Transaction)ledgerDataGrid.SelectedItem;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM transactions_T WHERE transaction_id=@transaction_id", conn);
                cmd.Parameters.AddWithValue("@transaction_id", sampleTransaction.transaction_id);
                cmd.ExecuteNonQuery();

            }
            else if (dialogResult == MessageBoxResult.No)
            {
                return;
            }

            conn.Close();

            MessageBox.Show("You have successfully deleted a record!");

            btnLedger_Click(new object(), new RoutedEventArgs());
        }

        private void btnSupplierRefresh_Click(object sender, RoutedEventArgs e)
        {
            btnSuppliers_Click(new object(), new RoutedEventArgs());
        }

        private void btnSupplierAdd_Click(object sender, RoutedEventArgs e)
        {
            SupplierAdd supplierAdd = new SupplierAdd();
            supplierAdd.Show();
        }

        private void btnSupplierEdit_Click(object sender, RoutedEventArgs e)
        {
            Supplier sampleSupplier = (Supplier)suppliersDataGrid.SelectedItem;
            SupplierEdit supplierEdit = new SupplierEdit(sampleSupplier.SupplierID);
            supplierEdit.Show();
        }
        private void btnSupplierDelete_Click(object sender, RoutedEventArgs e)
        {

            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                Supplier sampleSupplier = (Supplier)suppliersDataGrid.SelectedItem;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM suppliers_T WHERE supplier_id=@supplier_id", conn);
                cmd.Parameters.AddWithValue("@supplier_id", sampleSupplier.SupplierID);
                cmd.ExecuteNonQuery();

            }
            else if (dialogResult == MessageBoxResult.No)
            {
                return;
            }

            conn.Close();

            MessageBox.Show("You have successfully deleted a record!");

            btnSuppliers_Click(new object(), new RoutedEventArgs());
        }

        private void btnCustomerRefresh_Click(object sender, RoutedEventArgs e)
        {
            btnCustomers_Click(new object(), new RoutedEventArgs());
        }

        private void btnCustomerAdd_Click(object sender, RoutedEventArgs e)
        {
            CustomerAdd customerAdd = new CustomerAdd();
            customerAdd.Show();
        }


        private void btnCustomerEdit_Click(object sender, RoutedEventArgs e)
        {
            Customer sampleCustomer = (Customer)customersDataGrid.SelectedItem;
            CustomerEdit customerEdit = new CustomerEdit(sampleCustomer.CustomerID);
            customerEdit.Show();
        }

        private void btnCustomerDelete_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};", DBSERVER, DBNAME, DBUSER, DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open();

            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                Customer sampleCustomer= (Customer)customersDataGrid.SelectedItem;
                MySqlCommand cmd = new MySqlCommand("DELETE FROM customers_T WHERE customer_id=@customer_id", conn);
                cmd.Parameters.AddWithValue("@customer_id", sampleCustomer.CustomerID);
                cmd.ExecuteNonQuery();

            }
            else if (dialogResult == MessageBoxResult.No)
            {
                return;
            }

            conn.Close();

            MessageBox.Show("You have successfully deleted a record!");

            btnCustomers_Click(new object(), new RoutedEventArgs());
        }
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

    }
    
}
