using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public partial class ProgramCoOrdinator_AcademicManager : Window
    {
        public ProgramCoOrdinator_AcademicManager()
        {
            InitializeComponent();
        }
        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            string Username = txtUsername.Text;
            string UserPassword = txtPassword.Password;

            // Validate that both fields are filled out
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(UserPassword))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            try
            {
                // SQL connection string to  database
                string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

                using (SqlConnection connection = new SqlConnection(DBConn))
                {
                    connection.Open();

                    // SQL query to check if the email and password hash match an entry in the AccountUser table
                    string query = "SELECT COUNT(*) FROM Account WHERE Username = @Username AND UserPassword = @UserPassword AND AccountType = 'Programme Coordinator/Academic Manager';";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use SQL parameters to prevent SQL injection attacks
                        command.Parameters.AddWithValue("@Username", Username);
                        command.Parameters.AddWithValue("@UserPassword", UserPassword);  // Assuming password is stored as plain text (it should be hashed)

                        // Execute the query and get the number of matching entries
                        int count = (int)command.ExecuteScalar();

                        // Check if any entries were found
                        if (count > 0)
                        {
                            MessageBox.Show("Program Co-Ordinator/Academic Manager successfully logged in.");

                            // Open the Lecturer Dashboard window and close the login window
                            PCoordinatorAManagerDashboard pCoordinatorAManagerDashboard = new PCoordinatorAManagerDashboard();
                            pCoordinatorAManagerDashboard.Show();
                            this.Close();  // Close the login window
                        }
                        else
                        {
                            // No matching entries were found
                            MessageBox.Show("Invalid email or password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the process
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
