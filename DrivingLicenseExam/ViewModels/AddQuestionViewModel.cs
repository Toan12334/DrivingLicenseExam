using DrivingLicenseExam.Commands;
using DrivingLicenseExam.Data;
using DrivingLicenseExam.DTO;
using DrivingLicenseExam.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DrivingLicenseExam.ViewModels
{
    class AddQuestionViewModel
    {
        public event Action<QuestionDTO>? QuestionAdded;
        private readonly DrivingLicenseExamDbContext _db;
        private readonly int _examId;

        public QuestionDTO Question { get; set; }

        public AddQuestionViewModel(int examId)
        {
            _examId = examId;
            _db = new DrivingLicenseExamDbContext();
            Question = new QuestionDTO(); // NEW
        }



        private ICommand _confirmCommand;
        public ICommand ConfirmCommand
        {
            get
            {
                if (_confirmCommand == null)
                {
                    _confirmCommand = new RelayCommand(
                        () => AddQuestion(),
                        () => CanAdd()
                    );
                }
                return _confirmCommand;
            }
        }
        private bool CanAdd()
        {
            return !string.IsNullOrWhiteSpace(Question.content) &&
                   !string.IsNullOrWhiteSpace(Question.anSwerA) &&
                   !string.IsNullOrWhiteSpace(Question.anSwerB) &&
                   !string.IsNullOrWhiteSpace(Question.anSwerC) &&
                   !string.IsNullOrWhiteSpace(Question.anSwerD) &&
                   !string.IsNullOrWhiteSpace(Question.correctAnswer);
        }
        private void AddQuestion()
        {
            var question = new Question
            {
                Content = Question.content,
                AnswerA = Question.anSwerA,
                AnswerB = Question.anSwerB,
                AnswerC = Question.anSwerC,
                AnswerD = Question.anSwerD,
                CorrectAnswer = Question.correctAnswer,
                IsCritical = Question.isCritical
            };

            _db.Questions.Add(question);     
            _db.SaveChanges();

            _db.ExamQuestions.Add(new ExamQuestion
            {
                ExamId = _examId,
                QuestionId = question.Id
            });

            _db.SaveChanges();
           Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
            Question.questionId = question.Id;
            QuestionAdded?.Invoke(Question);

        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(
                        () => Cancel() // FIX: Pass a lambda expression to provide an Action
                    );
                }
                return _cancelCommand;
            }
        }
        private void Cancel()
        {
            // Đóng cửa sổ hiện tại
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }


        }
    }
}
