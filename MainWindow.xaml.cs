using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace PlayTechInventory
{
    public partial class MainWindow : Window
    {
        private const string DBSERVER = "localhost";
        private const string DBNAME = "db.playtech";
        private const string DBUSER = "chris";
        private const string DBPASS = "12345";
        public MainWindow()
        {
            InitializeComponent();

        }

        public void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};",DBSERVER,DBNAME,DBUSER,DBPASS);
            MySqlConnection conn = new MySqlConnection(connectionString);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM users_T WHERE username=@username AND password=@password", conn);
                cmd.Parameters.AddWithValue("@username", username.Text);
                cmd.Parameters.AddWithValue("@password", password.Password);
                MySqlDataReader reader = cmd.ExecuteReader();
                reader.Read();

                int count = Convert.ToInt32(reader.GetString("COUNT(*)"));

                if (count == 1)
                {
                    MessageBox.Show("You have logged in successfully!");
                    Dashboard dashboard = new Dashboard(username.Text);
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again!");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        public void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

}