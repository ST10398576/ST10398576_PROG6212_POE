using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ST10398576_PROG6212_POE
{
    /// <summary>
    /// Link to GitHub Repository: https://github.com/ST10398576/ST10398576_PROG6212_POE.git
    /// </summary>
    public partial class CreateAccount : Window
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void CancelAccount_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            string UserFirstName = txtUserFirstName.Text;
            string UserLastName = txtUserLastName.Text;
            string UserEmail = txtUserEmail.Text;
            string UserPhoneNumber = Convert.ToString(txtUserPhoneNumber.Text);
            string UserFaculty = txtUserFaculty.Text;
            string Username = txtUsername.Text;
            string UserPassword = txtUserPassword.Password;
            string UserConfirmPassword = txtConfirmUserPassword.Password;
            string? AccountType = (AccountTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(UserFirstName) || string.IsNullOrWhiteSpace(UserLastName) || string.IsNullOrWhiteSpace(UserEmail) ||
                string.IsNullOrWhiteSpace(UserPhoneNumber) || string.IsNullOrWhiteSpace(UserFaculty) || string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(UserPassword) || string.IsNullOrWhiteSpace(UserConfirmPassword) || string.IsNullOrWhiteSpace(AccountType))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }

            if (UserPhoneNumber.Length != 10)
            {
                MessageBox.Show("Please make sure The Phone Number only has 10 digits.");
                return;
            }

            if (UserConfirmPassword == UserPassword)
            {
                try
                {
                    // Address of SQL server and database 
                    string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

                    // Establish connection
                    using (SqlConnection con = new SqlConnection(DBConn))
                    {
                        // Open Connection
                        con.Open();

                        // SQL Query with all required fields, including email
                        string query = "INSERT INTO Account(UserFirstName, UserLastName, UserEmail, UserPhoneNumber, UserFaculty, Username, UserPassword, AccountType) " +
                                        "VALUES (@UserFirstName, @UserLastName, @UserEmail, @UserPhoneNumber, @UserFaculty, @Username, @UserPassword, @AccountType)";

                        // Execute query with parameters
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@UserFirstName", UserFirstName);
                            cmd.Parameters.AddWithValue("@UserLastName", UserLastName);
                            cmd.Parameters.AddWithValue("@UserEmail", UserEmail);
                            cmd.Parameters.AddWithValue("@UserPhoneNumber", UserPhoneNumber);
                            cmd.Parameters.AddWithValue("@UserFaculty", UserFaculty);
                            cmd.Parameters.AddWithValue("@Username", Username);
                            cmd.Parameters.AddWithValue("@UserPassword", UserPassword);
                            cmd.Parameters.AddWithValue("@AccountType", AccountType);

                            cmd.ExecuteNonQuery();
                        }

                        // Close Connection
                        con.Close();
                    }

                    MessageBox.Show($"{AccountType} account successfully created.");
                    Close();
                    MainWindow main = new MainWindow();
                    main.Show();
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Database error: " + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please make sure the Password and Confirm Password Fields are the same.");
                return;
            }
        }
    }
}
