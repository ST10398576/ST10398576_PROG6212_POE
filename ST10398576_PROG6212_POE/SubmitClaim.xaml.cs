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
            if (int.TryParse(txtLessonNum.Text, out int ClaimLessonNum) && decimal.TryParse(txtHourlyRate.Text, out decimal ClaimHourlyRate))
            {
                decimal ClaimTotalAmount = ClaimLessonNum * ClaimHourlyRate;
                txtLessonNum.Text = $"R {ClaimTotalAmount:F2}"; // Display in currency format
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
                string ClaimClassTaught = txtClassTaught.Text;
                int ClaimLessonNum = int.Parse(txtLessonNum.Text);
                decimal ClaimHourlyRate = decimal.Parse(txtHourlyRate.Text);
                string ClaimSupDocs = txtSupDocFile.Text;
                string ClaimStatus = "Pending";

            // Validate inputs
            if (string.IsNullOrEmpty(ClaimClassTaught) || ClaimLessonNum <= 0 || ClaimHourlyRate<=0 || string.IsNullOrEmpty(ClaimSupDocs))
            {
                MessageBox.Show("Please fill in all fields correctly and upload a document.");
                return;
            }
                
            decimal ClaimTotalAmount = ClaimLessonNum * ClaimHourlyRate;

                using (SqlConnection conn = new SqlConnection(DBConn))
                {
                    conn.Open();

                    string query = "INSERT INTO Claims (AccountID, ClaimClassTaught, ClaimLessonNum, ClaimHourlyRate, ClaimTotalAmount, ClaimSupDocs, ClaimStatus) " +
                                    "VALUES (@AccountID, @ClaimClassTaught, @ClaimLessonNum, @ClaimHourlyRate, @ClaimTotalAmount, @ClaimSupDocs, @ClaimStatus)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AccountID", 1);
                        cmd.Parameters.AddWithValue("@ClaimClassTaught", ClaimClassTaught);
                        cmd.Parameters.AddWithValue("@ClaimLessonNum", ClaimLessonNum);
                        cmd.Parameters.AddWithValue("@ClaimHourlyRate", ClaimHourlyRate);
                        cmd.Parameters.AddWithValue("@ClaimTotalAmount", ClaimTotalAmount);
                        cmd.Parameters.AddWithValue("@ClaimSupDocs", ClaimSupDocs);
                        cmd.Parameters.AddWithValue("@ClaimStatus", ClaimStatus);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Claim submitted successfully!");
                    // Optionally, reset the form for a new claim submission
                    ResetForm();
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

        private void ResetForm()
        {
            txtClassTaught.Text = "";
            txtLessonNum.Text = "";
            txtHourlyRate.Text = "";
            txtSupDocFile.Text = "";
            selectedFilePath = string.Empty;
        }

    }
}