using DrivingLicenseExam.Commands;
using DrivingLicenseExam.DTO;
using DrivingLicenseExam.Models;
using DrivingLicenseExam.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DrivingLicenseExam.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly DrivingLicenseExamDbContext _db;

        public ICommand LoginCommand { get; }

        #region Bind Properties
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _examCode;
        public string ExamCode
        {
            get => _examCode;
            set
            {
                _examCode = value;
                OnPropertyChanged(nameof(ExamCode));
            }
        }
        #endregion

        public LoginViewModel()
        {
            _db = new DrivingLicenseExamDbContext();
            LoginCommand = new RelayCommand(Login, CanLogin);
        }

        // =========================
        // CAN EXECUTE LOGIN
        // =========================
        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Username)
                && !string.IsNullOrWhiteSpace(Password)
                && !string.IsNullOrWhiteSpace(ExamCode);
        }

        // =========================
        // LOGIN FLOW
        // =========================
        private void Login()
        {
            // 1️⃣ Authenticate user
            var user = _db.Users
                .AsNoTracking()
                .FirstOrDefault(u =>
                    u.Username == Username &&
                    u.Password == Password
                );

            if (user == null)
            {
                MessageBox.Show("Sai username hoặc password");
                return;
            }

            // 2️⃣ Validate exam code
            var exam = _db.Exams
                .AsNoTracking()
                .FirstOrDefault(e =>
                    e.ExamCode.Trim() == ExamCode.Trim() &&
                    e.IsActive == true
                );

            if (exam == null)
            {
                MessageBox.Show("Mã thi không hợp lệ hoặc đề thi đã bị đóng");
                return;
            }

            // 3️⃣ Load questions by ExamId
            var questions = _db.ExamQuestions
                .AsNoTracking()
                .Where(eq => eq.ExamId == exam.Id)
                .Select(eq => new QuestionDTO
                {
                    questionId = eq.Question.Id,
                    content = eq.Question.Content,
                    anSwerA = eq.Question.AnswerA,
                    anSwerB = eq.Question.AnswerB,
                    anSwerC = eq.Question.AnswerC,
                    anSwerD = eq.Question.AnswerD,
                    correctAnswer = eq.Question.CorrectAnswer,
                    isCritical = eq.Question.IsCritical
                })
                .ToList();

            if (!questions.Any())
            {
                MessageBox.Show("Đề thi chưa có câu hỏi");
                return;
            }

            // 4️⃣ Open exam window
            var examWindow = new FormExamWindow( user,  exam,  questions);
            var examDoingViewModel = new ExamDoingViewModel(user, exam, questions);
            examWindow.Show();

            // 5️⃣ Close login window
            Application.Current.MainWindow?.Close();
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
