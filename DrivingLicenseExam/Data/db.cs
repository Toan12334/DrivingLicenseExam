using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DrivingLicenseExam.Data
{
    public class db : DbContext
    {
        public db()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Đọc appsettings.json
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                optionsBuilder.UseSqlServer(
                    config.GetConnectionString("DrivingLicenseExamDB")
                );
            }
        }
    }
}
