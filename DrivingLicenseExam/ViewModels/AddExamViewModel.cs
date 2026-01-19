using DrivingLicenseExam.Commands;
using DrivingLicenseExam.DTO;
using DrivingLicenseExam.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DrivingLicenseExam.ViewModels
{
    class AddExamViewModel : INotifyPropertyChanged
    {
        private readonly DrivingLicenseExamDbContext _db;
        private ExamDTO _exam;
        private RelayCommand _addExamCommand;
        private readonly ObservableCollection<ExamDTO> _examList;
        public event PropertyChangedEventHandler? PropertyChanged;

        public AddExamViewModel(ObservableCollection<ExamDTO> ExamList)
        {
            _db = new DrivingLicenseExamDbContext();
            Exam = new ExamDTO(); // ⭐ QUAN TRỌNG
            _examList = ExamList;
        }

        public ExamDTO Exam
        {
            get => _exam;
            set
            {
                _exam = value;
                OnPropertyChanged();
         
            }
        }

        public ICommand AddExamCommand =>
            _addExamCommand ??= new RelayCommand(AddExam, CanAddExam);

        private bool CanAddExam()
        {
            return !string.IsNullOrWhiteSpace(Exam.NameExam)
                && !string.IsNullOrWhiteSpace(Exam.ExamCode)
                && Exam.TimeLimit > 0;
        }

        private void AddExam()
        {
            var exam = new Exam
            {
                Name = Exam.NameExam,
                ExamCode = Exam.ExamCode,
                TimeLimit = Exam.TimeLimit,
                IsActive = false
            };

            _db.Exams.Add(exam);
            _db.SaveChanges();


            _examList.Add(new ExamDTO
            {
                Id = exam.Id,
                NameExam = exam.Name,
                ExamCode = exam.ExamCode,
                TimeLimit = exam.TimeLimit,
                NumberQuestion = 0,
                IsActive = false
            });
            // Clear form sau khi thêm
            Exam = new ExamDTO();
            MessageBox.Show("Exam added successfully!");
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
