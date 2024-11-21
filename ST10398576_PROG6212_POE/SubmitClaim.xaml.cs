using System.Data.SqlClient;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ST10398576_PROG6212_POE
{
    /// <summary>
    /// Link to GitHub Repository: https://github.com/ST10398576/ST10398576_PROG6212_POE.git
    /// </summary>
    public partial class SubmitClaim : Window
    {
        // Declare selectedFilePath as a member variable
        private string? selectedFilePath;

        public SubmitClaim()
        {
            InitializeComponent();
        }

        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";
        private List<string> uploadedFileNames = new List<string>();


        private void btnSupDoc_Click(object sender, RoutedEventArgs e)
        {
            // Open a file dialog for selecting a document
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Document files (*.pdf;*.docx;*.xlsx)|*.pdf;*.docx;*.xlsx|All files (*.*)|*.*"; // File type filters
            openFileDialog.Title = "Select Supporting Document";

            if (openFileDialog.ShowDialog() == true)
            {
                // Store the selected file path
                selectedFilePath = openFileDialog.FileName;
                // Update UI to display selected file name
                txtSupDocFile.Text = System.IO.Path.GetFileName(selectedFilePath);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtLessonNum.Text, out int noOfSessions) && decimal.TryParse(txtHourlyRate.Text, out decimal hourlyRate))
            {
                decimal totalAmount = noOfSessions * hourlyRate;
                txtLessonNum.Text = $"R {totalAmount:F2}"; // Display in currency format
            }
            else
            {
                txtLessonNum.Text = "Invalid input";
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                // Gets Claim data from the form
                string ClaimClassTaught = txtClassTaughtNum.Text;
                int ClaimLessonNum = int.Parse(txtLessonNum.Text);
                decimal ClaimHourlyRate = decimal.Parse(txtHourlyRate.Text);
                decimal ClaimTotalAmount = decimal.Parse(txtTotalClaimAmount.Text);
                string ClaimSupDocs = txtSupDocFile.Text;
                string ClaimStatus = "Pending";

            // Validate inputs
            if (string.IsNullOrEmpty(ClaimClassTaught) || ClaimLessonNum <= 0 || ClaimHourlyRate<=0 || string.IsNullOrEmpty(ClaimSupDocs))
            {
                MessageBox.Show("Please fill in all fields correctly and upload a document.");
                return;
            }


            SubmitClaimToDatabase(ClaimClassTaught, HoursWorked, HourlyRate, ClaimTotalAmount, ClaimSupDocs, ClaimStatus);

        }

        private void SubmitClaimToDatabase(string ClaimClassTaught, int ClaimLessonNum, int ClaimHourlyRate, string ClaimTotalAmount, string ClaimSupDocs, string ClaimStatus)
        {
            string query = "INSERT INTO Claims (ClaimClassTaught, ClaimLessonNum, ClaimHourlyRate, ClaimTotalAmount, ClaimSupDocs, ClaimStatus) VALUES (@ClaimClassTaught, @ClaimLessonNum, @ClaimHourlyRate, @ClaimTotalAmount, @ClaimSupDocs, @ClaimStatus)";

            using (SqlConnection conn = new SqlConnection(DBConn))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ClaimClassTaught", ClaimClassTaught);
                cmd.Parameters.AddWithValue("@ClaimLessonNum", ClaimLessonNum);
                cmd.Parameters.AddWithValue("@ClaimHourlyRate", ClaimHourlyRate);
                cmd.Parameters.AddWithValue("@ClaimTotalAmount", ClaimTotalAmount);
                cmd.Parameters.AddWithValue("@ClaimSupDocs", ClaimSupDocs);
                cmd.Parameters.AddWithValue("@ClaimStatus", ClaimStatus);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Claim submitted successfully!");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while submitting the claim: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        private bool VerifyDatabaseConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBConn))
                {
                    conn.Open(); // Try to open the connection
                    return true; // Connection is successful
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Connection error: {sqlEx.Message}");
                return false; // Connection failed
            }
        }

    }
}