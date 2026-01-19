using System;
using System.Collections.Generic;

namespace DrivingLicenseExam.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
