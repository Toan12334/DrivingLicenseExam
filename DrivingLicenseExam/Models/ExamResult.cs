using System;
using System.Collections.Generic;

namespace DrivingLicenseExam.Models;

public partial class ExamResult
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ExamId { get; set; }

    public int Score { get; set; }

    public bool IsPassed { get; set; }

    public DateTime SubmittedAt { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
