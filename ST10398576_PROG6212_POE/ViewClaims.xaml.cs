using System;
using System.Collections.Generic;
using System.Data;
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
    public partial class ViewClaims : Window
    {
        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

        public ViewClaims()
        {
            InitializeComponent();
            LoadClaimStatusViewer();
        }

        private void LoadClaimStatusViewer()
        {
            string query = "SELECT ClaimID, ClaimClassTaught, ClaimLessonNum, ClaimHourlyRate, ClaimTotalAmount, ClaimStatus FROM Claims"; // Adjust the query as necessary

            using (SqlConnection connection = new SqlConnection(DBConn))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                ClaimsListView.ItemsSource = dataTable.DefaultView; // Set the data source for the ListView
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
}