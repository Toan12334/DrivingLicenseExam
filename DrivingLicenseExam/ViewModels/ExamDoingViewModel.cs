using DrivingLicenseExam.Commands;
using DrivingLicenseExam.DTO;
using DrivingLicenseExam.Enums;
using DrivingLicenseExam.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Input;

namespace DrivingLicenseExam.ViewModels
{
    class ExamDoingViewModel : INotifyPropertyChanged

    {
        private bool flag ;
        private readonly Exam _exam;
        private readonly ExamTimerService examTimerService;
        private readonly User _user;
        private readonly List<QuestionDTO> _questions;
        private ICommand _submitCommand;
        private int _currentQuestionIndex;
        private DrivingLicenseExamDbContext _db= new DrivingLicenseExamDbContext();
        private string _timeRemaining;
   

       
        public ExamDoingViewModel(User user, Exam exam, List<QuestionDTO> questions)
        {
            _user = user;
            _exam = exam;
            _questions = questions;
            
            _currentQuestionIndex = 0;

            NextCommand = new RelayCommand(
                () => CurrentQuestionIndex++,
                () => CurrentQuestionIndex < TotalQuestions - 1
            );

            PreviousCommand = new RelayCommand(
                () => CurrentQuestionIndex--,
                () => CurrentQuestionIndex > 0
            );
            examTimerService = new ExamTimerService(_exam.TimeLimit);

            examTimerService.TimeUpdated += time =>
            {
                TimeRemaining = time.ToString(@"mm\:ss");
            };

            examTimerService.TimeExpired += () =>
            {
                MessageBox.Show("Hết giờ làm bài!");
                ExecuteSubmit();
            };

            examTimerService.Start();
        }
        public string TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                if (_timeRemaining != value)
                {
                    _timeRemaining = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                {
                    _submitCommand = new RelayCommand(
                        ExecuteSubmit,
                        CanExecuteSubmit
                    );
                }
                return _submitCommand;
            }
        }
        private bool CanExecuteSubmit()
        {
            return _questions.TrueForAll(q => q.SelectedAnswer != AnswerOption.None);
        }

        private void ExecuteSubmit()
        {
            int score = CalculateScore();
            ExamResult result = new ExamResult
            {
                UserId = _user.Id,
                ExamId = _exam.Id,
                Score = score,
                IsPassed = score >= 9 && !flag,
                SubmittedAt = DateTime.Now
            };
            _db.ExamResults.Add(result);
            _db.SaveChanges();

            MessageBox.Show(
                $"Bạn đã hoàn thành bài thi!\n" +
                $"Điểm của bạn: {score}/{TotalQuestions}\n" +
                $"{(result.IsPassed ? "Chúc mừng bạn đã vượt qua kỳ thi!" : "Rất tiếc, bạn chưa vượt qua kỳ thi.")}",
                "Kết quả thi",
                MessageBoxButton.OK,

                MessageBoxImage.Information
            );
            Application.Current.MainWindow.Close();

        }


        private int CalculateScore()
        {
            int score = 0;
            flag = false; // reset cờ

            foreach (var question in _questions)
            {
                bool isCorrect =
                    question.SelectedAnswer.ToString()
                    .Equals(question.correctAnswer, StringComparison.OrdinalIgnoreCase);

                if (isCorrect)
                {
                    score += 1;
                }

                // gắn cờ nếu sai câu liệt
                if (question.isCritical && !isCorrect)
                {
                    flag = true;
                }
            }

            return score;
        }

        #region Properties

        public int TotalQuestions => _questions.Count;

        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex;
            set
            {
                if (_currentQuestionIndex != value)
                {
                    _currentQuestionIndex = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CurrentQuestion));

                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
                    
        public QuestionDTO CurrentQuestion
            => _questions.Count > 0 ? _questions[_currentQuestionIndex] : null;

        #endregion

        #region Commands

        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
