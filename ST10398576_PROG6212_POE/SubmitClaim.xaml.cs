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
        public SubmitClaim()
        {
            InitializeComponent();
        }

        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

        private List<string> uploadedFileNames = new List<string>();


        private void btnSupDoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Title = "Select Documents",
                    Filter = "PDF Files (*.pdf)|*.pdf|Word Documents (*.docx;*.doc)|*.docx;*.doc|Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    foreach (string file in openFileDialog.FileNames)
                    {
                        StoreFileSecurely(file);
                    }
                    txtSupDoc.Text = string.Join(", ", uploadedFileNames);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void StoreFileSecurely(string filePath)
        {
            // Define the directory to store the uploaded files
            string directoryPath = "C:\\SecureUploads"; // Change this to your secure directory

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Generate a unique file name to avoid conflicts
            string fileName = Path.GetFileName(filePath);
            string newFilePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + fileName);

            // Copy the file to the secure directory
            File.Copy(filePath, newFilePath, true); // true to overwrite if the file already exists

            // Add the uploaded file name to the list
            uploadedFileNames.Add(fileName);
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            decimal TotalAmount = Convert.ToInt32(txtLessonNum.Text) * Convert.ToDecimal(txtHourlyRate.Text);
            txtTotalClaimAmount.Text = $"R{TotalAmount}";
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string ClaimClassTaught = txtClassTaughtNum.Text;
            string ClaimLessonNum = txtLessonNum.Text;
            string ClaimHourlyRate = txtHourlyRate.Text;
            string ClaimTotalAmount = txtTotalClaimAmount.Text;
            string ClaimSupDocs = txtSupDoc.Text;
            string ClaimStatus = "Pending";

            // Validate inputs
            if (string.IsNullOrEmpty(ClaimHourlyRate) && string.IsNullOrEmpty(ClaimHourlyRate))
            {
                MessageBox.Show("Please fill in hours worked and hourly rate.");
                return;
            }

            if (!int.TryParse(ClaimHourlyRate, out int HoursWorked) || !int.TryParse(ClaimHourlyRate, out int HourlyRate))
            {
                MessageBox.Show("Please enter valid numeric values for hours worked and hourly rate.");
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