using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    public partial class LecturerDashboard : Window
    {
        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog=PROG6212_POE;Integrated Security=True;";

        public LecturerDashboard(string Username)
        {
            InitializeComponent();
        }

        private void btnSubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            SubmitClaim submitClaim = new SubmitClaim();
            submitClaim.Show();
        }

        private void btnViewClaims_Click(object sender, RoutedEventArgs e)
        {
            ViewClaims viewClaims = new ViewClaims();
            viewClaims.Show();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            Close();
        }
    }
}
