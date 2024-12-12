using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uts.Models
{
    public class Courses
    {
        public int courseId { get; set; } = 0;
        public string? name { get; set; }
        public string? imageName { get; set; }
        public double? duration { get; set; }
        public string? description { get; set; }
        public Categories Category { get; set; }

        // New properties
        public int EnrollmentCount { get; set; } = 0;
        public string? InstructorName { get; set; }
    }
}
