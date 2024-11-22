using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ST10398576_PROG6212_POE
{
    /// <summary>
    /// Interaction logic for HRView.xaml
    /// </summary>
    public partial class HRView : Window
    {
        public HRView()
        {
            InitializeComponent();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = @"Data Source=labG9AEB3\SQLEXPRESS;Initial Catalog=PROG6212_POE;Integrated Security=True";
                string reportData = "Approved Claims Report\n\n";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT c.ClaimID, c.AccountID, u.UserEmail, c.ClaimClassTaught, c.ClaimLessonNum, c.ClaimHourlyRate," +
                                   "(c.ClaimLessonNum * c.ClaimHourlyRate) AS ClaimTotalAmount " + "FROM Claims c " +
                                    "INNER JOIN Account u ON c.AccountID = u.AccountID " + "WHERE c.ClaimStatus = 'Approved'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportData += $"Claim ID: {reader["ClaimID"]}\n" +
                                          $"Lecturer ID: {reader["AccountID"]}\n" +
                                          $"Email: {reader["UserEmail"]}\n"+
                                          $"Class Taught: {reader["ClaimClassTaught"]}\n" +
                                          $"Number of Sessions: {reader["ClaimLessonNum"]}\n" +
                                          $"Hourly Rate: {reader["ClaimHourlyRate"]:C}\n" +
                                          $"Total Amount: {reader["ClaimTotalAmount"]:C}\n\n";
                        }
                    }
                }

                // Prompt user to save the report
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    Title = "Save Report",
                    FileName = "ApprovedClaimsReport.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, reportData);
                    MessageBox.Show($"Report saved successfully at: {saveFileDialog.FileName}");
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void SearchLecturer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Email = txtSearchEmail.Text;

                if (string.IsNullOrEmpty(Email))
                {
                    MessageBox.Show("Please enter an email to search.");
                    return;
                }

                string connectionString = @"Data Source=labG9AEB3\SQLEXPRESS;Initial Catalog=PROG6212_POE;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT UserFirstName, UserLastName, UserEmail, UserPhoneNumber FROM Account WHERE UserEmail = @UserEmail";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserEmail", Email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFirstName.Text = reader["UserFirstName"].ToString();
                                txtLastName.Text = reader["UserLastName"].ToString();
                                txtEmail.Text = reader["UserEmail"].ToString();
                                txtPhoneNumber.Text = reader["UserPhoneNumber"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("User not found.");
                                ResetLecturerFields();
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void UpdateLecturer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string UserEmail = txtEmail.Text;
                string UserFirstName = txtFirstName.Text;
                string UserLastName = txtLastName.Text;
                string UserPhoneNumber = txtPhoneNumber.Text;

                if (string.IsNullOrEmpty(UserFirstName) || string.IsNullOrEmpty(UserLastName) || string.IsNullOrEmpty(UserEmail) || string.IsNullOrEmpty(UserPhoneNumber))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                string connectionString = @"Data Source=labG9AEB3\SQLEXPRESS;Initial Catalog=PROG6212_POE;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Account SET UserFirstName = @UserFirstName, UserLastName = @UserLastName, UserPhoneNumber = @UserPhoneNumber WHERE UserEmail = @UserEmail";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserEmail", UserEmail);
                        cmd.Parameters.AddWithValue("@UserFirstName", UserFirstName);
                        cmd.Parameters.AddWithValue("@UserLastName", UserLastName);
                        cmd.Parameters.AddWithValue("@UserPhoneNumber", UserPhoneNumber);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(" Information updated successfully!");
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ResetLecturerFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPhoneNumber.Text = "";
        }

    }
}
