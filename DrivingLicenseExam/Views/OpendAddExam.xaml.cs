using DrivingLicenseExam.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DrivingLicenseExam.ViewModels;
namespace DrivingLicenseExam.Views
{
    /// <summary>
    /// Interaction logic for OpendAddExam.xaml
    /// </summary>
    public partial class OpendAddExam : Window
    {
        public OpendAddExam(ObservableCollection<ExamDTO> ExamList)
        {
            InitializeComponent();
            DataContext = new ViewModels.AddExamViewModel(ExamList);
        }

       
    }
}
