using System.Windows;

namespace DrivingLicenseExam.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // Cập nhật password vào ViewModel
        private void PwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                // Replace 'pwdBox' with 'PwdBox' to match the field defined in the class
                ((ViewModels.LoginViewModel)this.DataContext).Password = PwdBox.Password;
               
            }
        }
    }
}
