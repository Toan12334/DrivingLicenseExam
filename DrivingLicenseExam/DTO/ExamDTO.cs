using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrivingLicenseExam.DTO
{
    class ExamDTO
    {
        public int Id { get; set; }          // 👈 BẮT BUỘC
        public string NameExam { get; set; }
        public int TimeLimit { get; set; }
        public int NumberQuestion { get; set; }
        public required string ExamCode { get; set; }
        public bool IsActive { get; set; }
    }
}
