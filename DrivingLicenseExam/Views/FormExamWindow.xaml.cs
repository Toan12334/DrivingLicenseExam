using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DrivingLicenseExam.DTO;
using DrivingLicenseExam.Models;
using DrivingLicenseExam.ViewModels;

namespace DrivingLicenseExam.Views
{
    public partial class FormExamWindow : Window
    {
        public FormExamWindow(User user,Exam exam,List<QuestionDTO> questions)
        {
            InitializeComponent();
            DataContext = new ExamDoingViewModel(user, exam, questions);
        }
    }
}
