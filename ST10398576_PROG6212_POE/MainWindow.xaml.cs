using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10398576_PROG6212_POE
{
    /// <summary>
    /// Link to GitHub Repository: https://github.com/ST10398576/ST10398576_PROG6212_POE.git
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string DBConn = "Data Source=labg9aeb3\\sqlexpress;Initial Catalog = PROG6212_POE; Integrated Security = True;";

        private void LecturerSignIn_Click(object sender, RoutedEventArgs e)
        {
            Lecturer lecturer = new Lecturer();
            lecturer.Show();
        }

        private void PCoordinatorAManagerSignIn_Click(object sender, RoutedEventArgs e)
        {
            ProgramCoOrdinator_AcademicManager programCoOrdinator_AcademicManager = new ProgramCoOrdinator_AcademicManager();
            programCoOrdinator_AcademicManager.Show();
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            createAccount.Show();
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}