using DrivingLicenseExam.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrivingLicenseExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                using (var context = new db())
                {
                    MessageBox.Show("Kết nối DB thành công!");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi DB: " + ex.Message);
            }
        }
    }
}