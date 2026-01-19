using DrivingLicenseExam.Data;
using DrivingLicenseExam.DTO;
using DrivingLicenseExam.Models;
using DrivingLicenseExam.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DrivingLicenseExam.Commands;


namespace DrivingLicenseExam.ViewModels
{
    class LoadExamQuestionViewModel : INotifyPropertyChanged
    {
        private DrivingLicenseExamDbContext _db;
        private QuestionDTO _selectedQuestion;
        private ObservableCollection<QuestionDTO> _questions;
        private int _examId;
        private QuestionDTO _newQuestion;
        public QuestionDTO NewQuestion
        {
            get => _newQuestion;
            set
            {
                _newQuestion = value;
                OnPropertyChanged(nameof(NewQuestion));
            }
        }

        public ObservableCollection<QuestionDTO> Questions
        {
           
            get => _questions;
            set
            {
                _questions = value;
                OnPropertyChanged(nameof(Questions));
            }
        }

        public QuestionDTO SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                OnPropertyChanged(nameof(SelectedQuestion));
            }
        }

        // 👉 Constructor NHẬN ExamId
        public LoadExamQuestionViewModel(int examId)
        {
            _examId = examId;
            _db = new DrivingLicenseExamDbContext();            
            LoadQuestions(examId);
          
            NewQuestion = new QuestionDTO(); // 👈 form Add
        }

        // 👉 Load question theo ExamId
        private void LoadQuestions(int examId)
        {
            var data = _db.ExamQuestions
                .AsNoTracking()
                .Where(eq => eq.ExamId == examId)
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

            Questions = new ObservableCollection<QuestionDTO>(data);
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        SaveQuestion,
                        CanSaveQuestion
                    );
                }
                return _saveCommand;
            }
        }



        private bool CanSaveQuestion()
        {
            return SelectedQuestion != null;
        }
        private void SaveQuestion()
        {
            var question = _db.Questions.Find(SelectedQuestion.questionId);
            if (question != null)
            {
                question.Content = SelectedQuestion.content;
                question.AnswerA = SelectedQuestion.anSwerA;
                question.AnswerB = SelectedQuestion.anSwerB;
                question.AnswerC = SelectedQuestion.anSwerC;
                question.AnswerD = SelectedQuestion.anSwerD;
                question.CorrectAnswer = SelectedQuestion.correctAnswer;
                question.IsCritical = SelectedQuestion.isCritical;
                _db.SaveChanges();
               MessageBox.Show("Cập nhật câu hỏi thành công!");
               
            }
            
        }

        private ICommand  _addCommand;
        public ICommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(
                        openAdd,
                        CanOpen
                    );
                }
                return _addCommand;
            }
        }
       
        private bool CanOpen()
        {
            return true;
        }
        private void openAdd()
        {
            var addQuestionWindow = new OpenAddQuestionWindow(_examId);
            addQuestionWindow.ShowDialog();
            // Reload questions after adding a new one
            LoadQuestions(_examId);
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(
                       DeleteQuestion,
                       CanDeleteQuestion
                    );
                }
                return _deleteCommand;
            }
        }
        private bool CanDeleteQuestion()
        {
            return SelectedQuestion != null;
        }
        private void DeleteQuestion()
        {
            var question = _db.Questions.Find(SelectedQuestion.questionId);
            if (question != null)
            {
                _db.Questions.Remove(question);
                _db.SaveChanges();
                Questions.Remove(SelectedQuestion);
                MessageBox.Show("Xóa câu hỏi thành công!");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
