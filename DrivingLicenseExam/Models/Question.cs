using System;
using System.Collections.Generic;

namespace DrivingLicenseExam.Models;

public partial class Question
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public string AnswerA { get; set; } = null!;

    public string AnswerB { get; set; } = null!;

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }

    public string CorrectAnswer { get; set; } = null!;

    public bool IsCritical { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
}
