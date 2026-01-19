using DrivingLicenseExam.Commands;
using DrivingLicenseExam.Data;
using DrivingLicenseExam.DTO;
using DrivingLicenseExam.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using DrivingLicenseExam.Views;
using DrivingLicenseExam.ViewModels;

class ExamManagementViewModel : INotifyPropertyChanged
{
    private readonly DrivingLicenseExamDbContext _db;

    private ExamDTO _selectedExam;
    private ObservableCollection<ExamDTO> _examList;

    public ObservableCollection<ExamDTO> ExamList
    {
        get => _examList;
        set
        {
            _examList = value;
            OnPropertyChanged(nameof(ExamList));
        }
    }

    public ExamDTO SelectedExam
    {
        get => _selectedExam;
        set
        {
            _selectedExam = value;
            OnPropertyChanged(nameof(SelectedExam));

            // 👉 cập nhật trạng thái nút
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public ICommand ViewQuestionsCommand { get; }
    public ICommand OpenAddCommand { get; }

    public ExamManagementViewModel()
    {
        _db = new DrivingLicenseExamDbContext();

        ViewQuestionsCommand = new RelayCommand(
            ViewQuestions,
            () => SelectedExam != null
        );
        OpenAddCommand = new RelayCommand(OpenAddExamWindow);

        LoadExams();
    }
    private void OpenAddExamWindow()
    {
        var window = new OpendAddExam(ExamList);
        var viewModel = new AddExamViewModel(ExamList);
        window.Show();
    }


    private void ViewQuestions()
    {
        var window = new LoadExamQuestionWindow(SelectedExam.Id);
       
       
        window.Show();
    }

    private void LoadExams()
    {
        ExamList = new ObservableCollection<ExamDTO>(
            _db.Exams
                .AsNoTracking()
                .Select(e => new ExamDTO
                {
                    Id = e.Id,
                    NameExam = e.Name,
                    TimeLimit = e.TimeLimit,
                    ExamCode = e.ExamCode,
                    NumberQuestion = e.ExamQuestions.Count(),
                    IsActive=e.IsActive
                })
                .ToList()
        )
        {

        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
