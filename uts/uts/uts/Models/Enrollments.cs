using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uts.Models
{
    public class Enrollments
    {
        public int EnrollmentId { get; set; }
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
