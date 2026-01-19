using DrivingLicenseExam.ViewModels;
using System;
using System.Collections.Generic;
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

namespace DrivingLicenseExam.Views
{
    /// <summary>
    /// Interaction logic for OpenAddQuestionWindow.xaml
    /// </summary>
    public partial class OpenAddQuestionWindow : Window
    {
        public OpenAddQuestionWindow(int examId)
        {
            InitializeComponent();
            DataContext = new AddQuestionViewModel(examId);
        }
    }

}
