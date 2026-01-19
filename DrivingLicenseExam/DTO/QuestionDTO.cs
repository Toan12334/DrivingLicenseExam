using DrivingLicenseExam.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class QuestionDTO : INotifyPropertyChanged
{
    private bool _isCritical;
    public int questionId { get; set; }
    public string content { get; set; }
    public string anSwerA { get; set; }
    public string anSwerB { get; set; }
    public string anSwerC { get; set; }
    public string anSwerD { get; set; }
    public string correctAnswer { get; set; }
    public bool isCritical
    {
        get => _isCritical;
        set
        {
            _isCritical = value;
            OnPropertyChanged(nameof(isCritical));
        }
    }

    private AnswerOption _selectedAnswer = AnswerOption.None;

    
    public AnswerOption SelectedAnswer
    {
        get => _selectedAnswer;
        set
        {
            if (_selectedAnswer != value)
            {
                _selectedAnswer = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
