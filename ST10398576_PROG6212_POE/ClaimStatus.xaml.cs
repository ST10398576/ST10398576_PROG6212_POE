using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
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
    public partial class ClaimStatus : Window
    {
        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

        public ClaimStatus()
        {
            InitializeComponent();
            LoadClaims();
        }

        private void LoadClaims()
        {
            // Update the query to select the new fields
            string query = "INSERT INTO Claims (AccountID, ClaimClassTaught, ClaimLessonNum, ClaimHourlyRate, ClaimTotalAmount, ClaimSupDocs, ClaimStatus) " +
                           "VALUES (@AccountID, @ClaimClassTaught, @ClaimLessonNum, @ClaimHourlyRate, @ClaimTotalAmount, @ClaimSupDocs, @ClaimStatus)";

            List<Claim> claims = new List<Claim>();
            using (SqlConnection conn = new SqlConnection(DBConn))
            {
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        claims.Add(new Claim
                        {
                            ClaimID = reader.GetInt32(0),
                            ClaimClassTaught = reader.GetString(1),
                            ClaimLessonNum = reader.GetInt32(2),
                            ClaimHourlyRate = reader.GetInt32(3),
                            ClaimTotalAmount = reader.GetString(4),
                            ClaimStatus = reader.GetString(5)
                        });
                    }

                    // Check if claims list is empty
                    if (claims.Count == 0)
                    {
                        MessageBox.Show("You have no claims submitted.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while loading claims: {ex.Message}");
                }
            }

            ClaimsListView.ItemsSource = claims;
        }
        public class Claim
        {
            public int ClaimID { get; set; }
            public string ClaimClassTaught { get; set; }
            public int ClaimLessonNum { get; set; }
            public int ClaimHourlyRate { get; set; }
            public string ClaimTotalAmount { get; set; }
            public string ClaimStatus { get; set; }
        }
    }
}