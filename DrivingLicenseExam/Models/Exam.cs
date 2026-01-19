using System;
using System.Collections.Generic;

namespace DrivingLicenseExam.Models;

public partial class Exam
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TimeLimit { get; set; }
    public string ExamCode { get; set; } = null!;
    public bool IsActive { get; set; }

    public virtual ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();

    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
