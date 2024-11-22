using PROG6212_POE;
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
    public partial class PCoordinatorAManagerDashboard : Window
    {
        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

        public PCoordinatorAManagerDashboard()
        {
            InitializeComponent();
            GetClaimsFromDatabase();
            LoadClaims(); 
        }

        //// Method to load claims into the ListView
        private void LoadClaims()
        {
            List<Claim> dbClaims = GetClaimsFromDatabase();
            ClaimsListView.ItemsSource = dbClaims;

            List<Claim> claims = new List<Claim>();
            using (SqlConnection conn = new SqlConnection(DBConn))
            {
                conn.Open();
                string query = "SELECT ClaimID, ClaimClassTaught, ClaimLessonNum, ClaimHourlyRate, ClaimStatus FROM Claims WHERE ClaimStatus = 'Pending'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var claim = new Claim
                            {
                                ClaimID = (int)reader["ClaimID"],
                                ClaimClassTaught = reader["ClaimClassTaught"]?.ToString() ?? string.Empty, // Safe access with null check
                                ClaimLessonNum = (int)reader["ClaimLessonNum"],
                                ClaimHourlyRate = (decimal)reader["ClaimHourlyRate"],
                                ClaimStatus = reader["ClaimStatus"]?.ToString() ?? string.Empty, // Safe access with null check
                                ClaimTotalAmount = (int)reader["ClaimLessonNum"] * (decimal)reader["ClaimHourlyRate"]
                            };

                            // Apply automated verification criteria
                            if (VerifyClaimCriteria(claim))
                            {
                                // Automatically approve claims that meet the criteria
                                UpdateClaimStatus(claim.ClaimID, "Approved");
                                claim.ClaimStatus = "Approved";
                            }
                            else
                            {
                                // Automatically reject claims that fail to meet criteria
                                UpdateClaimStatus(claim.ClaimID, "Rejected");
                                claim.ClaimStatus = "Rejected";
                            }

                            claims.Add(claim);
                        }
                    }
                }
            }
        }

        // Method to verify if a claim meets predefined criteria
        private bool VerifyClaimCriteria(Claim claim)
        {
            // Define your criteria; for example:
            int maxSessions = 20;
            decimal maxHourlyRate = 500m;

            // Check if claim meets the criteria
            return claim.ClaimLessonNum <= maxSessions && claim.ClaimHourlyRate <= maxHourlyRate;
        }



        //calls the claims and displays them in a listed view
        private List<Claim> GetClaimsFromDatabase()
        {
            List<Claim> claims = new List<Claim>();
            string query = "select ClaimID, ClaimClassTaught, ClaimTotalAmount, ClaimStatus from Claims where AccountID = 1";

            using (SqlConnection connection = new SqlConnection(DBConn))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        claims.Add(new Claim
                        {
                            ClaimID = reader.GetInt32(0),
                            ClaimClassTaught = reader.GetString(1),
                            ClaimTotalAmount = reader.GetDecimal(2),
                            ClaimStatus = reader.GetString(3)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error: " + ex.Message);
                }
            }
            return claims;
        }


        //updates the claims status based on coordinators decision
        private void UpdateClaimStatus(int claimID, string newStatus)
        {
            string query = "UPDATE Claims SET ClaimStatus = @ClaimStatus WHERE ClaimID = @ClaimID";

            using (SqlConnection connection = new SqlConnection(DBConn))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ClaimStatus", newStatus);
                command.Parameters.AddWithValue("@ClaimID", claimID);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Claim status updated successfully!");
                    //LoadClaims(); // Reload claims after updating
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            // Reload claims after updating
            LoadClaims();
        }

        private void HR_View_Click(object sender, RoutedEventArgs e)
        {
            HRView hRView = new HRView();
            hRView.Show();
        }

        //Approve button changes status of claims to approved
        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClaimsListView.SelectedItem is Claim selectedClaim)
            {
                UpdateClaimStatus(selectedClaim.ClaimID, "Approved");
            }
            else
            {
                MessageBox.Show("Please select a claim to approve.");
            }
        }
        //rejection button will change status to rejected
        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClaimsListView.SelectedItem is Claim selectedClaim)
            {
                UpdateClaimStatus(selectedClaim.ClaimID, "Rejected");
            }
            else
            {
                MessageBox.Show("Please select a claim to reject.");
            }
        }
        //pending button will change status to pending
        private void PendingButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClaimsListView.SelectedItem is Claim selectedClaim)
            {
                UpdateClaimStatus(selectedClaim.ClaimID, "Pending");
            }
            else
            {
                MessageBox.Show("Please select a claim to set as pending.");
            }
        }

        
    }

    public class Claim
    {
        public int ClaimID { get; set; }
        public string ClaimClassTaught { get; set; }
        public int ClaimLessonNum { get; set; }
        public decimal ClaimHourlyRate { get; set; }
        public decimal ClaimTotalAmount { get; set; }
        public string ClaimStatus { get; set; }
    }
}
